using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exeptions;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(IUserService userService, IUserProfileService profileService)
        : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IUserProfileService _profileService = profileService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            try
            {
                await _userService.RegisterAsync(
                    request.FirstName,
                    request.Email,
                    request.Password,
                    request.Role
                );
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            try
            {
                string token = await _userService.LoginAsync(request.Email, request.Password);
                return Ok(token);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [Authorize]
        [HttpGet("role")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                string token = GetTokenFromHeaders();
                string role = await _userService.GetRoleAsync(token);
                return Ok(role);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileResponse>> Get()
        {
            try
            {
                string token = GetTokenFromHeaders();
                UserProfileResponse user = await _userService.GetFromTokenAsync(token);
                return Ok(user);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("profile/{id:guid}")]
        public async Task<ActionResult<UserProfileResponse>> Get(Guid id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            try
            {
                string token = GetTokenFromHeaders();
                Guid id = await _userService.GetIdFromTokenAsync(token);

                await _userService.UpdateAsync(
                    id,
                    request.LastName,
                    request.FirstName,
                    request.MiddleName,
                    request.Description,
                    request.JobTitle
                );

                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [Authorize]
        [HttpGet("profile/many")]
        public async Task<ActionResult<UserProfileResponse>> Get([FromQuery] IList<Guid> ids)
        {
            try
            {
                return Ok(await _userService.GetManyAsync(ids));
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("member")]
        [HasPermission(Permission.BeMember)]
        public async Task<ActionResult<MemberResponse>> GetMember()
        {
            try
            {
                string token = GetTokenFromHeaders();
                MemberResponse member = await _profileService.GetMemberAsync(token);
                return Ok(member);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("member/{id:guid}")]
        public async Task<ActionResult<MemberResponse>> GetMember(Guid id)
        {
            try
            {
                MemberResponse member = await _profileService.GetMemberAsync(id);
                return Ok(member);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("organizer")]
        [HasPermission(Permission.BeOrganizer)]
        public async Task<ActionResult<OrganizerResponse>> GetOrganizer()
        {
            try
            {
                string token = GetTokenFromHeaders();
                OrganizerResponse organizer = await _profileService.GetOrganizerAsync(token);
                return Ok(organizer);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("organizer/{id:guid}")]
        public async Task<ActionResult<OrganizerResponse>> GetOrganizer(Guid id)
        {
            try
            {
                OrganizerResponse organizer = await _profileService.GetOrganizerAsync(id);
                return Ok(organizer);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("client")]
        [HasPermission(Permission.BeClient)]
        public async Task<ActionResult<ClientResponse>> GetClient()
        {
            try
            {
                string token = GetTokenFromHeaders();
                ClientResponse client = await _profileService.GetClientAsync(token);
                return Ok(client);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("client/{id:guid}")]
        public async Task<ActionResult<ClientResponse>> GetClient(Guid id)
        {
            try
            {
                ClientResponse client = await _profileService.GetClientAsync(id);
                return Ok(client);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            string token = GetTokenFromHeaders();
            await _userService.DeleteAsync(token);
            return Ok();
        }

        private string GetTokenFromHeaders() =>
            Request.Headers.Authorization.FirstOrDefault()!.Split(" ").Last();
    }
}
