using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Teams
{
    public record TeamResponse(
        Guid Id,
        string Name,
        string Description,
        IList<UserProfileResponse> Members,
        Guid AdminId
        );
}
