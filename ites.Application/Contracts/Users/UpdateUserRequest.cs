namespace ites.Application.Contracts.Users
{
    public record UpdateUserRequest(
        string LastName,
        string FirstName,
        string MiddleName,
        string Description,
        string? JobTitle
        );
}
