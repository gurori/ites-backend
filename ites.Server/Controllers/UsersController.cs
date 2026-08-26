using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(
    IUserService userService,
    IUserProfileService profileService,
    IConfiguration configuration
) : BaseController
{
    private readonly string _organizerConfirmKey =
        configuration["ORGANIZER_CONFIRM_KEY"]
        ?? throw new InvalidOperationException("Configuration 'ORGANIZER_CONFIRM_KEY' is missing.");

    [Authorize]
    [HttpPatch("me")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserRequest request,
        CancellationToken ct = default
    )
    {
        await userService.UpdateAsync(GetUserId(), request, ct);
        return NoContent();
    }

    [HttpGet("member/me")]
    [HasPermission(Permission.BeMember)]
    public async Task<IActionResult> GetCurrentMember(CancellationToken ct = default)
    {
        var member = await profileService.GetMemberAsync(GetUserId(), ct);
        return Ok(member);
    }

    [HttpGet("member/{id:guid}")]
    public async Task<IActionResult> GetMemberById(Guid id, CancellationToken ct = default)
    {
        var member = await profileService.GetMemberAsync(id, ct);
        return Ok(member);
    }

    [HttpGet("organizer/me")]
    [HasPermission(Permission.BeOrganizer)]
    public async Task<IActionResult> GetCurrentOrganizer(CancellationToken ct = default)
    {
        var organizer = await profileService.GetOrganizerAsync(GetUserId(), ct);
        return Ok(organizer);
    }

    [HttpGet("organizer/{id:guid}")]
    public async Task<IActionResult> GetOrganizerById(Guid id, CancellationToken ct = default)
    {
        var organizer = await profileService.GetOrganizerAsync(id, ct);
        return Ok(organizer);
    }

    [HttpPost("organizer/confirm")]
    public Task<IActionResult> ConfirmKey([FromBody] UserConfirmKeyRequest request)
    {
        return Task.Run<IActionResult>(() =>
        {
            if (string.IsNullOrWhiteSpace(request.Key))
                return BadRequest("Ключ не может быть пустым.");

            bool isValid = string.Equals(
                request.Key.Trim(),
                _organizerConfirmKey,
                StringComparison.Ordinal
            );

            return isValid ? Ok() : BadRequest("Неверный ключ подтверждения.");
        });
    }

    [HttpGet("client/me")]
    [HasPermission(Permission.BeClient)]
    public async Task<IActionResult> GetCurrentClient(CancellationToken ct = default)
    {
        var client = await profileService.GetClientAsync(GetUserId(), ct);
        return Ok(client);
    }

    [HttpGet("client/{id:guid}")]
    public async Task<IActionResult> GetClientById(Guid id, CancellationToken ct = default)
    {
        var client = await profileService.GetClientAsync(id, ct);
        return Ok(client);
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> Delete(CancellationToken ct = default)
    {
        await userService.DeleteAsync(GetUserId(), ct);
        return NoContent();
    }
}
