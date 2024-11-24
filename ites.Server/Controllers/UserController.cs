using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(
        IUserService userService,
        IUserProfileService profileService)
            : BaseController
    {
        private readonly IUserService _userService = userService;
        private readonly IUserProfileService _profileService = profileService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request) =>
            await TryCatchAsync(async () =>
            {
                await _userService
                    .RegisterAsync(request.FirstName, request.Email, request.Password, request.Role);
                return Ok();
            });

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request) =>
            await TryCatchAsync(async () =>
            {
                string token = await _userService
                    .LoginAsync(request.Email, request.Password);
                Response.Cookies.Append("auth", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
                return Ok();
            });

        [Authorize]
        [HttpGet("role")]
        public async Task<IActionResult> GetRole() =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                string role = await _userService.GetRoleAsync(token);
                return Ok(role);
            });

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Get() =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                UserProfileResponse user = await _userService
                    .GetFromTokenAsync(token);
                return Ok(user);
            });

        [HttpGet("profile/{id:guid}")]
        public async Task<IActionResult> Get(Guid id) =>
            await TryCatchAsync(async () =>
            {
                var user = await _userService.GetAsync(id);
                return Ok(user);
            });

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserRequest request) =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                Guid id = await _userService.GetIdFromTokenAsync(token);

                await _userService.UpdateAsync(id,
                                               request.LastName,
                                               request.FirstName,
                                               request.MiddleName,
                                               request.Description,
                                               request.JobTitle);

                return Ok();
            });

        [Authorize]
        [HttpGet("profile/many")]
        public async Task<IActionResult> Get([FromQuery] IList<Guid> ids) =>
            await TryCatchAsync(async () =>
            {
                return Ok(await _userService.GetManyAsync(ids));
            });

        [HttpGet("member")]
        [HasPermission(Permission.BeMember)]
        public async Task<IActionResult> GetMember() =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                MemberResponse member = await _profileService
                    .GetMemberAsync(token);
                return Ok(member);
            });

        [HttpGet("member/{id:guid}")]
        public async Task<IActionResult> GetMember(Guid id) =>
            await TryCatchAsync(async () =>
            {
                MemberResponse member = await _profileService
                    .GetMemberAsync(id);
                return Ok(member);
            });

        [HttpGet("organizer")]
        [HasPermission(Permission.BeOrganizer)]
        public async Task<IActionResult> GetOrganizer() =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                OrganizerResponse organizer = await _profileService
                    .GetOrganizerAsync(token);
                return Ok(organizer);
            });

        [HttpGet("organizer/{id:guid}")]
        public async Task<IActionResult> GetOrganizer(Guid id) =>
            await TryCatchAsync(async () =>
            {
                OrganizerResponse organizer = await _profileService
                    .GetOrganizerAsync(id);
                return Ok(organizer);
            });

        [HttpGet("client")]
        [HasPermission(Permission.BeClient)]
        public async Task<IActionResult> GetClient() =>
            await TryCatchAsync(async () =>
            {
                string token = GetTokenFromHeaders();
                ClientResponse client = await _profileService
                    .GetClientAsync(token);
                return Ok(client);
            });

        [HttpGet("client/{id:guid}")]
        public async Task<IActionResult> GetClient(Guid id) =>
            await TryCatchAsync(async () =>
            {
                ClientResponse client = await _profileService
                    .GetClientAsync(id);
                return Ok(client);
            });
    }
}