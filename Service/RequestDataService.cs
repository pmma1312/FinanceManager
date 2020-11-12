using FinanceManager.Infrastructure.Model;
using FinanceManager.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace FinanceManager.Service
{
    public interface IRequestDataService
    {
        Task<User> GetCurrentUser();
    }

    public class RequestDataService : IRequestDataService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IUserRepository _userRepository;
        private User _currentUser;

        public RequestDataService(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = contextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetCurrentUser()
        {
            return _currentUser ?? await LoadCurrentUser();
        }

        private async Task<User> LoadCurrentUser()
        {
            User currentUser = new User();

            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenString))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString.ToString().Replace("Bearer ", ""));

                currentUser = await _userRepository.GetById(long.Parse(token.Payload["userId"].ToString()));

                _currentUser = currentUser;
            }

            return currentUser;
        }
    }
}
