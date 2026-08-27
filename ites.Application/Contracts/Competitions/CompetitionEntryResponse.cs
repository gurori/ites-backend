using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts;

public sealed record CompetitionEntryResponse(
    Guid Id,
    MemberSummaryResponse FromMember,
    CompetitionSummaryResponse ForCompetition
);
