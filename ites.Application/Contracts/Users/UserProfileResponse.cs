namespace ites.Application.Contracts.Users
{
    public record UserProfileResponse(
        Guid Id,
        string FirstName,
        string Email,
        string LastName,
        string MiddleName,
        string Description
        );
}
