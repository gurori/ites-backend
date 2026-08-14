using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ites.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IUserService userService, IOptions<JwtOptions> jwtOptions)
    : BaseController
{
    private readonly IUserService _userService = userService;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        await _userService.RegisterAsync(
            request.FirstName,
            request.Email,
            request.Password,
            request.Role
        );

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        LoginUserResponse loginResponse = await _userService.LoginAsync(
            request.Email,
            request.Password
        );

        SetCookie("auth", loginResponse.Token, _jwtOptions.Expires);
        SetCookie("role", loginResponse.Role, _jwtOptions.Expires);

        return Ok(new { role = loginResponse.Role });
    }

    private void SetCookie(string name, string value, int expiresDays)
    {
        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(expiresDays),
        };

        cookieOptions.Extensions.Add("Partitioned");

        Response.Cookies.Append(name, value, cookieOptions);
    }
}
