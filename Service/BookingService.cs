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
    public interface IBookingService
    {
        public Task<BaseResponse> Create(BookingDto booking);
        public Task<BaseResponse> Get();
        public Task<BaseResponse> GetByCategoryId(long categoryId);
    }

    public class BookingService : IBookingService
    {
        private IBookingRepository _bookingRepository;
        private ICategoryRepository _categoryRepository;
        private IRequestDataService _requestDataService;

        public BookingService(IBookingRepository bookingRepository, IRequestDataService requestDataService, ICategoryRepository categoryRepository)
        {
            _bookingRepository = bookingRepository;
            _categoryRepository = categoryRepository;
            _requestDataService = requestDataService;
        }

        public async Task<BaseResponse> Get()
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            response.Data.Add("bookings", await _bookingRepository.GetBookingsForUser(currentUser.UserId));

            return response;
        }

        public async Task<BaseResponse> GetByCategoryId(long categoryId)
        {
            var response = new BaseResponse();

            User currentUser = await _requestDataService.GetCurrentUser();

            var bookings = await _bookingRepository.GetBookingsWithCategory(categoryId, currentUser.UserId);

            response.Data.Add("bookings", bookings);

            return response;
        }

        public async Task<BaseResponse> Create(BookingDto booking) 
        {
            var response = new BaseResponse();
            
            User currentUser = await _requestDataService.GetCurrentUser();

            var dbCategory = await _categoryRepository.GetById(booking.BookingCategoryId);

            if(dbCategory is null)
            {
                response.Infos.Errors.Add($"No category found with id {booking.BookingCategoryId}");
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            if(dbCategory.CategoryOwner.UserId != currentUser.UserId)
            {
                response.Infos.Errors.Add($"You can only create bookings for categories that belong to you");
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            BookingDtoValidator validator = new BookingDtoValidator();
            var result = await validator.ValidateAsync(booking);

            if(!result.IsValid)
            {
                response.Infos.Errors.AddRange(result.Errors.ToList().Select(error => error.ErrorMessage));
                response.StatusCode = HttpStatusCode.UnprocessableEntity;
                return response;
            }

            Booking newBooking = new Booking
            {
                BookingAmount = booking.BookingAmount,
                BookingCategory = dbCategory,
                BookingDate = DateTime.Now,
                BookingUser = currentUser,
                BookingDescription = booking.BookingDescription
            };

            var responseBooking = await _bookingRepository.Insert(newBooking);
            responseBooking.BookingUser.Password = null;

            response.Data.Add("booking", responseBooking);

            return response;
        }

    }
}
