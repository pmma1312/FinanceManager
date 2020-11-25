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
    [Route("api/balance/")]
    public class MonthlyBalanceController : ControllerBase
    {
        private IMonthlyBalanceService _monthlyBalanceService;

        public MonthlyBalanceController(IMonthlyBalanceService monthlyBalanceService)
        {
            _monthlyBalanceService = monthlyBalanceService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Create(MonthlyBalanceDto monthlyBalance)
        {
            var response = await _monthlyBalanceService.Create(monthlyBalance);
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Update(MonthlyBalanceDto monthlyBalance)
        {
            var response = await _monthlyBalanceService.Update(monthlyBalance);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Get()
        {
            var response = await _monthlyBalanceService.Get();
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpGet("spendings")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetSpendings()
        {
            var response = await _monthlyBalanceService.GetSpendings();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("revenue")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetRevenue()
        {
            var response = await _monthlyBalanceService.GetRevenue();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("categories")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> GetForEachCategory()
        {
            var response = await _monthlyBalanceService.GetForEachCategory();
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
