using ites.Core.Enums;
using ites.Core.Exeptions;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private const string _bearerPrefix = "Bearer ";

        protected string GetJwtFromHeaders()
        {
            var authorization = Request.Headers.Authorization.ToString();

            if (
                string.IsNullOrWhiteSpace(authorization)
                || !authorization.StartsWith(_bearerPrefix, StringComparison.OrdinalIgnoreCase)
            )
                throw new UnauthorizedException();

            return authorization[_bearerPrefix.Length..].Trim();
        }

        protected Guid GetUserId()
        {
            string userIdString =
                User.FindFirst(CustomClaims.UserId)?.Value ?? throw new UnauthorizedException();

            if (!Guid.TryParse(userIdString, out var userId))
                throw new UnauthorizedException();

            return userId;
        }
    }
}
