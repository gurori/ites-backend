namespace ites.Application.Contracts.Users;

public sealed record UpdateUserRequest(
    string LastName,
    string FirstName,
    string MiddleName,
    string Description,
    string? JobTitle
);
