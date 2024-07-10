using System.ComponentModel.DataAnnotations;

namespace ites.Application.Contracts.Users
{
    public record RegisterUserRequest(
        [Required] string FirstName,
        [Required] string Email,
        [Required] string Password,
        [Required] string Role
        );
}

