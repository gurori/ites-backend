using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Exeptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class UserController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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

        [HttpGet("file/{userId:guid}/{fileName}")]
        public async Task<IActionResult> GetFile(Guid userId, string fileName)
        {
            try
            {
                byte[] fileBytes = await _userService
                    .GetFileAsync(_webHostEnvironment.WebRootPath, userId, fileName);
                string fileExtension = Path
                    .GetExtension(fileName).TrimStart('.');
                return File(fileBytes, GetMimeType(fileExtension));
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
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
                    id, request.LastName, request.FirstName, request.MiddleName, request.Description);

                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        private string GetTokenFromHeaders() =>
            Request.Headers.Authorization
                    .FirstOrDefault()!.Split(" ").Last();

        private static string GetMimeType(string fileExtension)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { "png", "image/png" },
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
            };

            if (mimeTypes.TryGetValue(fileExtension.ToLower(), out var mimeType))
                return mimeType;

            return "application/octet-stream";
        }
    }
}
