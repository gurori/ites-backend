namespace ites.Application.Contracts.Users;

public sealed record RegisterUserRequest(
    string FirstName,
    string Email,
    string Password,
    string Role
);
