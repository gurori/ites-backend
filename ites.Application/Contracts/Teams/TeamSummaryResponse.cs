using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Teams;

public sealed record TeamSummaryResponse(
    Guid Id,
    string Name,
    string Description,
    IList<UserProfileResponse> Members,
    Guid AdminId
);