using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceManager.Controllers
{
    [ApiController]
    [Route("api/booking/")]
    public class BookingController : ControllerBase
    {
        private IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
               
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Create(BookingDto booking)
        {
            var response = await _bookingService.Create(booking);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{year}/{month}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Get(int year, int month)
        {
            // Gets bookings in specified period
            var response = await _bookingService.Get(year, month);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetAll()
        {
            var response = await _bookingService.GetAll();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{bookingId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Delete(long bookingId)
        {
            var response = await _bookingService.Delete(bookingId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("category/{categoryId}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetByCategoryId(long categoryId)
        {
            var response = await _bookingService.GetByCategoryId(categoryId);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
