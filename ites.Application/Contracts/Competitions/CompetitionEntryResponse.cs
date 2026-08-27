using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Competitions;

public sealed record CompetitionEntryResponse(
    Guid Id,
    MemberSummaryResponse FromMember,
    CompetitionSummaryResponse ForCompetition
);
