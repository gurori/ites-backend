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
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken ct = default
    )
    {
        await userService.RegisterAsync(request, ct);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken ct = default
    )
    {
        var loginResponse = await userService.LoginAsync(request, ct);

        SetAuthCookie("auth", loginResponse.Token);
        SetAuthCookie("role", loginResponse.Role);

        return Ok(new { role = loginResponse.Role });
    }

    private void SetAuthCookie(string name, string value)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.Expires),
        };

        cookieOptions.Extensions.Add("Partitioned");

        Response.Cookies.Append(name, value, cookieOptions);
    }
}
