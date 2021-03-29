using WinterwoodStock.Library.Models;

namespace WinterwoodStock.Library.Interfaces.Services
{
    public interface IAuthService
    {
        AuthModel GetToken(AuthModel authModel);

        AuthModel Register(AuthModel authModel);
    }
}
