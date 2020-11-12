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

    }
}
