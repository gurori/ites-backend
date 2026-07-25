using ites.Core.Exeptions;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetTokenFromCookies()
        {
            string? token = Request.Cookies["auth"] ?? throw new UnauthorizedException();
            
            return token;
        }
    }
}
