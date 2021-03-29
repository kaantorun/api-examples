using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterwoodStock.Library.Models;

namespace WinterwoodStockApi.Middlewares
{
    public class SetCurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public SetCurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var bodyStr = "";
            var req = context.Request;

            // Allows using several time the stream in ASP.Net Core
            //req.EnableRewind();

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            AuthModel authModel = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthModel>(bodyStr);

            //AuthController authController = new AuthController();


            // Rewind, so the core is not lost when it looks the body for the request
            //req.Body.Position = 0;

            // Do whatever work with bodyStr here
            string token = context.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(token))
            {
                await _next(context);
            }

            var handler = new JwtSecurityTokenHandler();
            var tokenWithoutBearer = token.Split(' ')[1];
            var jwtToken = handler.ReadToken(tokenWithoutBearer) as JwtSecurityToken;
            var userName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "username")?.Value;

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
