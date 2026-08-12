using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

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

        return Ok(loginResponse);
    }
}
