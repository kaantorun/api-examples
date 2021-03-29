using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WinterwoodStock.Library.Interfaces.Services;
using WinterwoodStock.Library.Models;

namespace WinterwoodStockApi.Controllers
{
    [Authorize]
    [Route("WinterwoodStock/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// According to the username and password
        /// system creates a token if the user exists
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult RequestToken([FromBody] AuthModel authModel)
        {
            AuthModel token = _authService.GetToken(authModel);

            return Ok(token);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("registerauth")]
        public IActionResult Register([FromBody] AuthModel authModel)
        {
            AuthModel token = _authService.Register(authModel);

            return Ok(authModel);

        }
    }
}
