using ites.Application.Contracts.Teams;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TeamsController(ITeamService teamService) : BaseController
{
    [HttpPost]
    [HasPermission(Permission.CreateTeam)]
    public async Task<IActionResult> Create(
        [FromBody] TeamRequest request,
        CancellationToken ct = default
    )
    {
        var id = await teamService.CreateAsync(GetUserId(), request, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100,
        CancellationToken ct = default
    )
    {
        var teams = await teamService.GetAllAsync(page, pageSize, ct);
        return Ok(teams);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var team = await teamService.GetAsync(id, ct);
        return Ok(team);
    }

    [HttpPost("{teamId:guid}/join-requests")]
    [HasPermission(Permission.AddTeamApplication)]
    public async Task<IActionResult> AddJoinRequest(
        Guid teamId,
        [FromBody] AddTeamJoinRequestDto request,
        CancellationToken ct = default
    )
    {
        var id = await teamService.AddJoinRequestAsync(GetUserId(), teamId, request, ct);
        return Ok(id);
    }

    [HttpPatch("join-requests/{joinRequestId:guid}/handle")]
    [HasPermission(Permission.HandleTeamApplication)]
    public async Task<IActionResult> HandleJoinRequest(
        Guid joinRequestId,
        [FromBody] HandleTeamJoinRequestDto request,
        CancellationToken ct = default
    )
    {
        await teamService.HandleJoinRequestAsync(GetUserId(), joinRequestId, request, ct);
        return NoContent();
    }
}
