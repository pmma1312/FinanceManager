using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceManager.Controllers
{
    [ApiController]
    [Route("api/user/")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<BaseResponse>> Authenticate(LoginDto user)
        {
            var response = await _userService.Authenticate(user);
            return StatusCode((int) response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Register(RegistrationDto user)
        {
            var response = await _userService.Register(user);
            return StatusCode((int)response.StatusCode, response);
        }

    }
}
