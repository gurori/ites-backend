using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Teams;

public sealed record TeamResponse(
    Guid Id,
    string Name,
    string Description,
    IReadOnlyCollection<MemberSummaryResponse> Members,
    Guid AdminId
);
