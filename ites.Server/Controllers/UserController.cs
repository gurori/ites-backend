using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Exeptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(IUserService userService)
        : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            try
            {
                await _userService
                    .RegisterAsync(request.FirstName, request.Email, request.Password, request.Role);
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
                string token = await _userService
                    .LoginAsync(request.Email, request.Password);
                HttpContext.Response.Cookies
                    .Append("auth", token);
                return Ok();
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
            catch(ApiException ex)
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
                UserProfileResponse user = await _userService
                    .GetFromTokenAsync(token);
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

                await _userService.UpdateAsync(id,
                                               request.LastName,
                                               request.FirstName,
                                               request.MiddleName,
                                               request.Description,
                                               request.JobTitle);

                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        private string GetTokenFromHeaders() =>
            Request.Headers.Authorization
                    .FirstOrDefault()!
                    .Split(" ")
                    .Last();
    }
}
