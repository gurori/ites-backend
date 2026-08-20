using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(IUserService userService, IUserProfileService profileService)
        : BaseController
    {
        private readonly IUserService _userService = userService;
        private readonly IUserProfileService _profileService = profileService;
        private static readonly string _orginizerConfirmKey =
            Environment.GetEnvironmentVariable("ORGANIZER_CONFIRM_KEY")
            ?? throw new Exception("Enviroment variable 'ORGANIZER_CONFIRM_KEY' is not found");

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Get()
        {
            Guid userId = GetUserId();
            UserProfileResponse user = await _userService.GetAsync(userId);
            return Ok(user);
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            Guid userId = GetUserId();

            await _userService.UpdateAsync(
                userId,
                request.LastName,
                request.FirstName,
                request.MiddleName,
                request.Description,
                request.JobTitle
            );

            return Ok();
        }

        [Authorize]
        [HttpGet("profile/many")]
        public async Task<IActionResult> Get([FromQuery] ICollection<Guid> ids)
        {
            return Ok(await _userService.GetManyAsync(ids));
        }

        [HttpGet("member")]
        [HasPermission(Permission.BeMember)]
        public async Task<IActionResult> GetMember()
        {
            Guid userId = GetUserId();
            MemberResponse member = await _profileService.GetMemberAsync(userId);
            return Ok(member);
        }

        [HttpGet("member/{id:guid}")]
        public async Task<IActionResult> GetMember(Guid id)
        {
            MemberResponse member = await _profileService.GetMemberAsync(id);
            return Ok(member);
        }

        [HttpGet("organizer")]
        [HasPermission(Permission.BeOrganizer)]
        public async Task<IActionResult> GetOrganizer()
        {
            Guid userId = GetUserId();
            OrganizerResponse organizer = await _profileService.GetOrganizerAsync(userId);
            return Ok(organizer);
        }

        [HttpGet("organizer/{id:guid}")]
        public async Task<IActionResult> GetOrganizer(Guid id)
        {
            OrganizerResponse organizer = await _profileService.GetOrganizerAsync(id);
            return Ok(organizer);
        }

        [HttpPost("organizer/confirm")]
        public async Task<IActionResult> ConfirmKey(UserConfirmKeyRequest request)
        {
            return await Task.FromResult<IActionResult>(
                request.Key.Trim() == _orginizerConfirmKey ? Ok() : Conflict()
            );
        }

        [HttpGet("client")]
        [HasPermission(Permission.BeClient)]
        public async Task<IActionResult> GetClient()
        {
            Guid userId = GetUserId();
            ClientResponse client = await _profileService.GetClientAsync(userId);
            return Ok(client);
        }

        [HttpGet("client/{id:guid}")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            ClientResponse client = await _profileService.GetClientAsync(id);
            return Ok(client);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _userService.DeleteAsync(GetUserId());
            return Ok();
        }
    }
}
