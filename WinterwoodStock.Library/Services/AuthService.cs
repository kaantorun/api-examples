using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Enums;
using WinterwoodStock.Library.Interfaces.Repositories;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;

namespace WinterwoodStock.Library.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthService> _logger;

        private readonly IUnitOfWork _uow;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger, IUnitOfWork uow)
        {
            _configuration = configuration;

            _logger = logger;

            _uow = uow;
        }

        public AuthModel GetToken(AuthModel authModel)
        {
            string token = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(authModel.Username) || string.IsNullOrEmpty(authModel.Password))
                {
                    authModel.Status = StatusMessage.Error;
                    authModel.Message = "Login Details Cannot be Empty!";
                    return authModel;
                }

                var claims = new[]
                {
                    new Claim("username", authModel.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expireMin = Convert.ToInt32(_configuration["AuthOptions:ExpiresMin"]);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["AuthOptions:Domain"],
                    audience: _configuration["AuthOptions:Domain"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expireMin),
                    signingCredentials: creds);

                token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                var user = _uow.Users.Find(x => x.UserName == authModel.Username && x.Password == authModel.Password).FirstOrDefault();
                if (user == null)
                {
                    _logger.LogWarning($"User cannot be found on DB -> Username:{authModel.Username}, Password:{authModel.Password}");

                    authModel.Status = StatusMessage.Error;
                    authModel.Message = "Username and/or Password is not correct!";

                    return authModel;
                }

                user.Token = token;
                user.ExpireDate = DateTime.UtcNow.AddMinutes(expireMin);

                _uow.Users.Update(user);

                _uow.Complete();

                authModel.Id = user.UserId;
                authModel.Token = user.Token;
                authModel.FirstName = user.FirstName;
                authModel.LastName = user.LastName;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetToken -> Exception:{Newtonsoft.Json.JsonConvert.SerializeObject(ex)}");

                authModel.Status = StatusMessage.Exception;
                authModel.Message = "There is a technical problem. Please try again later.";
            }

            return authModel;
        }

        public AuthModel Register(AuthModel authModel)
        {
            string token = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(authModel.Username) || string.IsNullOrEmpty(authModel.Password) ||
                    string.IsNullOrEmpty(authModel.FirstName) || string.IsNullOrEmpty(authModel.LastName))
                {
                    _logger.LogWarning($"User details are missing!");
                    authModel.Status = StatusMessage.Error;
                    authModel.Message = "Please fill all the user information!";

                    return authModel;
                }

                var claims = new[]
                {
                    new Claim("username", authModel.Username)
                };

                //same username exist
                var user = _uow.Users.Find(x => x.UserName == authModel.Username).FirstOrDefault();

                if (user != null)
                {
                    _logger.LogWarning($"User name exist, Username: {authModel.Username}");

                    authModel.Status = StatusMessage.Error;
                    authModel.Message = "Username has already been taken! Please choose a different username!";

                    return authModel;
                }

                _uow.Users.Add(new User
                {
                    FirstName = authModel.FirstName,
                    LastName = authModel.LastName,
                    UserName = authModel.Username,
                    Password = authModel.Password,
                    CreatedDateTime = DateTime.UtcNow
                });

                _uow.Complete();

                user = _uow.Users.Find(x => x.UserName == authModel.Username && x.Password == authModel.Password).FirstOrDefault();

                authModel.Id = user.UserId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Register -> Exception:{Newtonsoft.Json.JsonConvert.SerializeObject(ex)}");

                authModel.Status = StatusMessage.Exception;
                authModel.Message = "There is a technical problem. Please try again later.";
            }

            return authModel;
        }
    }
}
