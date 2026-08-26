using ites.Application.Contracts.Competitions;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CompetitionsController(ICompetitionsService competitionsService)
    : BaseController
{
    [HttpPost]
    [HasPermission(Permission.CreateCompetition)]
    public async Task<IActionResult> Create(
        [FromBody] CompetitionRequest request,
        CancellationToken ct = default
    )
    {
        var id = await competitionsService.CreateAsync(GetUserId(), request, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100,
        CancellationToken ct = default
    )
    {
        var competitions = await competitionsService.GetAllAsync(page, pageSize, ct);
        return Ok(competitions);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var competition = await competitionsService.GetAsync(id, ct);
        return Ok(competition);
    }

    [HttpPost("{competitionId:guid}/entries")]
    [HasPermission(Permission.AddCompetitionApplication)]
    public async Task<IActionResult> AddEntry(
        Guid competitionId,
        [FromBody] CompetitionEntryRequest request,
        CancellationToken ct = default
    )
    {
        var entryId = await competitionsService.AddEntryAsync(
            GetUserId(),
            competitionId,
            request,
            ct
        );
        return Ok(entryId);
    }

    [HttpPatch("entries/{entryId:guid}/handle")]
    [HasPermission(Permission.HandleCompetitionApplication)]
    public async Task<IActionResult> HandleEntry(
        Guid entryId,
        [FromBody] HandleCompetitionEntryRequest request,
        CancellationToken ct = default
    )
    {
        await competitionsService.HandleEntryAsync(GetUserId(), entryId, request, ct);
        return NoContent();
    }
}
