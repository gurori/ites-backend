namespace ites.Application.Contracts.Users;

public sealed record UserProfileResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string MiddleName,
    string Description,
    string JobTitle,
    string Role
);
