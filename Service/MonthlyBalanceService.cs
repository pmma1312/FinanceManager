using FinanceManager.Data.DataTransferObjects;
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
        public Task<BaseResponse> Get();
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

        public async Task<BaseResponse> Get()
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var bookings = await _bookingRepository.GetBookingsForMonth(DateTime.Now, currentUser.UserId);

            var balance = (await _monthlyBalanceRepository.GetNewestMonthlyBalance(currentUser.UserId)).AvailableMonthlyBalance;

            bookings.ForEach(booking =>
            {
                balance -= booking.BookingAmount;
            });

            response.Data.Add("balance", balance);

            return response;
        }

    }
}
