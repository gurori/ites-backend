using ites.Core.Exeptions;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid GetUserId()
    {
        string userIdString =
            User.FindFirst(ClaimNames.UserId)?.Value ?? throw new UnauthorizedException();

        if (!Guid.TryParse(userIdString, out var userId))
            throw new UnauthorizedException();

        return userId;
    }
}
