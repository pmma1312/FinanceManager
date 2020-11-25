using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Enum;
using FinanceManager.Data.Response;
using FinanceManager.Infrastructure.Model;
using FinanceManager.Infrastructure.Repository;
using FinanceManager.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinanceManager.Service
{
    public interface IMonthlyBalanceService
    {
        public Task<BaseResponse> Create(MonthlyBalanceDto monthlyBalance);
        public Task<BaseResponse> Update(MonthlyBalanceDto monthlyBalance);
        public Task<BaseResponse> Get();
        public Task<BaseResponse> GetSpendings();
        public Task<BaseResponse> GetRevenue();
        public Task<BaseResponse> GetForEachCategory();
    }

    public class MonthlyBalanceService : IMonthlyBalanceService
    {

        private IMonthlyBalanceRepository _monthlyBalanceRepository;
        private IBookingRepository _bookingRepository;
        private IRequestDataService _requestDataService;

        public MonthlyBalanceService(IMonthlyBalanceRepository monthlyBalanceRepository, IBookingRepository bookingRepository, IRequestDataService requestDataService)
        {
            _monthlyBalanceRepository = monthlyBalanceRepository;
            _bookingRepository = bookingRepository;
            _requestDataService = requestDataService;
        }

        public async Task<BaseResponse> Create(MonthlyBalanceDto monthlyBalance)
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var currentMonthlyBalance = await _monthlyBalanceRepository.GetNewestMonthlyBalance(currentUser.UserId);

            if(currentMonthlyBalance != null && currentMonthlyBalance.ValidUntil > DateTime.Now)
            {
                response.Infos.Errors.Add($"You already have a monthly balance until {currentMonthlyBalance.ValidUntil.ToString("dd.MM.yyyy")}. Please delete it first to set a new one.");
                response.StatusCode = HttpStatusCode.Conflict;
                return response;
            }

            MonthlyBalanceDtoValidator validator = new MonthlyBalanceDtoValidator();
            var result = validator.Validate(monthlyBalance);

            if(!result.IsValid)
            {
                response.Infos.Errors.AddRange(result.Errors.ToList().Select(error => error.ErrorMessage));
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            var newMonthlyBalance = new MonthlyBalance
            {
                ValidUntil = monthlyBalance.ValidUntil,
                AvailableMonthlyBalance = monthlyBalance.AvailableMonthlyBalance,
                BalanceUser = currentUser
            };

            var dbBalance = await _monthlyBalanceRepository.Insert(newMonthlyBalance);
            dbBalance.BalanceUser = null;

            response.Data.Add("balance", dbBalance);

            return response;
        }

        public async Task<BaseResponse> Update(MonthlyBalanceDto monthlyBalance)
        {
            var response = new BaseResponse();

            if(!monthlyBalance.MonthlyBalanceId.HasValue)
            {
                response.Infos.Errors.Add("Monthly BalanceId is required to update");
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            var dbBalance = await _monthlyBalanceRepository.GetById(monthlyBalance.MonthlyBalanceId.Value);

            if(dbBalance is null)
            {
                response.Infos.Errors.Add($"BalanceId {monthlyBalance.MonthlyBalanceId} has not been found");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            User currentUser = await _requestDataService.GetCurrentUser();

            if(dbBalance.BalanceUser.UserId != currentUser.UserId)
            {
                response.Infos.Errors.Add("You can only edit balances that belong to you");
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }

            MonthlyBalanceDtoValidator validator = new MonthlyBalanceDtoValidator();
            var validationResult = await validator.ValidateAsync(monthlyBalance);

            if(!validationResult.IsValid)
            {
                response.Infos.Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            dbBalance.AvailableMonthlyBalance = monthlyBalance.AvailableMonthlyBalance;
            dbBalance.ValidUntil = monthlyBalance.ValidUntil;

            if(!await _monthlyBalanceRepository.Update(dbBalance))
            {
                response.Infos.Errors.Add("The update has failed");
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }

            response.Data.Add("balance", dbBalance);

            return response;
        }

        public async Task<BaseResponse> Get()
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var bookings = await _bookingRepository.GetBookingsForMonth(DateTime.Now, currentUser.UserId);

            var balanceObject = await _monthlyBalanceRepository.GetNewestMonthlyBalance(currentUser.UserId);
            var balance = balanceObject.AvailableMonthlyBalance;

            bookings.ForEach(booking =>
            {
                if (booking.BookingType == BookingTypeEnum.Spending)
                    balance -= booking.BookingAmount;
                else
                    balance += booking.BookingAmount;
            });

            response.Data.Add("balance", Math.Round(balance, 2));
            response.Data.Add("balanceObject", balanceObject);

            return response;
        }

        public async Task<BaseResponse> GetForEachCategory()
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var bookings = await _bookingRepository.GetBookingsForMonth(DateTime.Now, currentUser.UserId);

            var categories = await _bookingRepository.GetBookingsForMonth(DateTime.Now, currentUser.UserId);

            categories = categories
                      .GroupBy(booking => booking.BookingCategory.CategoryName)
                      .Select(b => b.First())
                      .Select(booking =>
                      {
                          booking.BookingAmount = 0;

                          return booking;
                      })
                      .ToList();

            bookings.ForEach(booking =>
            {
                categories.ForEach(category =>
                {
                    if (category.BookingCategory.CategoryName == booking.BookingCategory.CategoryName) {
                        if (booking.BookingType == BookingTypeEnum.Spending)
                            category.BookingAmount -= booking.BookingAmount;
                        else
                            category.BookingAmount += booking.BookingAmount;
                    }
                });
            });

            var resultCategories = categories
                .Select(category => new { category.BookingCategory.CategoryName, category.BookingAmount });

            response.Data.Add("categoryBookings", resultCategories);

            return response;
        }

        public async Task<BaseResponse> GetSpendings()
        {
            var response = new BaseResponse();

            response.Data.Add("spendings", await GetRevenueOrSpendings(BookingTypeEnum.Spending));

            return response;
        }

        public async Task<BaseResponse> GetRevenue()
        {
            var response = new BaseResponse();

            response.Data.Add("revenue", await GetRevenueOrSpendings(BookingTypeEnum.Revenue));

            return response;
        }

        private async Task<float> GetRevenueOrSpendings(BookingTypeEnum bookingType)
        {
            float total = 0;

            User currentUser = await _requestDataService.GetCurrentUser();

            var bookings = await _bookingRepository.GetBookingsForMonth(DateTime.Now, currentUser.UserId);

            bookings.ForEach(booking =>
            {
                if (booking.BookingType == bookingType)
                {
                    total += booking.BookingAmount;
                }
            });

            return total;
        }

    }
}
