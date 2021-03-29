using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using WinterwoodStock.Library.Entities;
using WinterwoodStock.Library.Interfaces.Repositories;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;
using WinterwoodStock.Library.Repositories;
using WinterwoodStock.Library.Services;
using Xunit;

namespace WinterwoodStock.Test
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly Mock<ILogger<AuthService>> _logger;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly IConfiguration _configuration;

        public AuthServiceTests()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            _logger = new Mock<ILogger<AuthService>>();

            _uowMock = new Mock<IUnitOfWork>();

            _authService = new AuthService(_configuration, _logger.Object, _uowMock.Object);
        }

        #region GetToken

        [Fact]
        public void Should_GetToken_ValidParameter()
        {
            _uowMock.Setup(u => u.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new List<User> { new User { UserId = 1 } });
            _uowMock.Setup(u => u.Users.Update(It.IsAny<User>()));
            _uowMock.Setup(u => u.Complete()).Returns(1);

            var result = _authService.GetToken(new AuthModel { Username = "x", Password = "y" });
            Assert.True(result != null);
            Assert.True(result.Id == 1);
        }

        [Fact]
        public void Should_Not_GetToken_When_User_Not_Found()
        {
            var result = _authService.GetToken(new AuthModel { Username = null, Password = null });
            Assert.True(result.Id == 0 && result.Status == Library.Enums.StatusMessage.Error);
        }

        #endregion GetToken

        #region Register

        [Fact]
        public void Should_Register_ValidParameter()
        {
            _uowMock.Setup(u => u.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new List<User> { new User { UserName = "x", Password = "y", UserId = 1 } });
            var result = _authService.Register(new AuthModel { Username = "t", Password = "5", FirstName = "z", LastName = "q" });
            Assert.True(result.Id == 0 && result.Status == Library.Enums.StatusMessage.Error);

            _uowMock.Setup(u => u.Users.Add(new User { UserName = "xyz", Password = "abc", UserId = 2, FirstName="qwe", LastName="rty" }));
            _uowMock.Setup(u => u.Complete()).Returns(1);
            _uowMock.Setup(u => u.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new List<User> { new User { UserName = "xyz", Password = "abc", UserId = 2, FirstName = "qwe", LastName = "rty" } });
            Assert.True(result.Id == 2 && result.Status == Library.Enums.StatusMessage.Success);
        }

        [Fact]
        public void Should_Not_Register_When_User_Already_Registered()
        {
            _uowMock.Setup(u => u.Users.Find(It.IsAny<Expression<Func<User, bool>>>())).Returns(() => new List<User> { new User { UserName = "x", Password = "y", UserId = 1 } });
            var result = _authService.Register(new AuthModel { Username = "x", Password = "y", FirstName = "z", LastName = "q" });
            Assert.True(result.Id == 0 && result.Status == Library.Enums.StatusMessage.Error);
        }

        #endregion Register
    }
}
