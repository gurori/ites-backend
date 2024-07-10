using System.ComponentModel.DataAnnotations;

namespace ites.Application.Contracts.Users
{
    public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password
        );
}

