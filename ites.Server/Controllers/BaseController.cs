using ites.Core.Exeptions;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetTokenFromHeaders() =>
            Request.Headers.Authorization.FirstOrDefault()!.Split(" ").Last();
    }
}
