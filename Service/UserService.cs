using FinanceManager.Data.DataTransferObjects;
using FinanceManager.Data.Response;
using FinanceManager.Infrastructure.Model;
using FinanceManager.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Service
{
    public interface IUserService
    {
        public Task<BaseResponse> Authenticate(LoginDto user);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IConfiguration _config;

        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<BaseResponse> Authenticate(LoginDto user)
        {
            var response = new BaseResponse();

            var dbUser = await _userRepository.GetByUsername(user.Username);

            if(dbUser is null) return InvalidCredentialsResponse(response);

            if (dbUser.Password != user.Password) return InvalidCredentialsResponse(response);

            response.Data.Add("token", GenerateJSONWebToken(dbUser));

            return response;
        }

        private BaseResponse InvalidCredentialsResponse(BaseResponse response)
        {
            response.Infos.Errors.Add("Invalid username or password");
            response.StatusCode = HttpStatusCode.Unauthorized;

            return response;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, user.Username)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
