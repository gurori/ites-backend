using ites.Application.Constants;
using ites.Application.Contracts.Moderation;
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
    public async Task<IActionResult> Get(CancellationToken ct = default)
    {
        return Ok(await moderationService.GetAllAsync(ct));
    }

    [HttpPatch("orders/{id:guid}")]
    public async Task<IActionResult> HandleOrder(
        Guid id,
        ModerationRequest request,
        CancellationToken ct = default
    )
    {
        await moderationService.HandleAsync("order", id, request.Accept, ct);
        return NoContent();
    }

    [HttpPatch("teams/{id:guid}")]
    public async Task<IActionResult> Handle(
        Guid id,
        ModerationRequest request,
        CancellationToken ct = default
    )
    {
        await moderationService.HandleAsync("team", id, request.Accept, ct);
        return NoContent();
    }
}
