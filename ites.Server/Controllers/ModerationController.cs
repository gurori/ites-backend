using ites.Application.Constants;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[HasPermission(Permission.Moderate)]
public sealed class ModerationController(IModerationService moderationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await moderationService.GetAllAsync());
    }

    [HttpPost("{type}/{id:guid}/{accept:bool}")]
    public async Task<IActionResult> Handle(string type, Guid id, bool accept)
    {
        type = type.ToLower();
        if (type != ModerationTarget.Order || type != ModerationTarget.Team)
        {
            return BadRequest();
        }
        await moderationService.HandleAsync(type, id, accept);
        return Ok();
    }
}
