using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            var response = await _bookingService.Get();
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
