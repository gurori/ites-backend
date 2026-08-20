using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts;

public sealed record CompetitionApplicationResponse(
    Guid Id,
    UserProfileResponse FromMember,
    CompetitionResponse ForCompetition
);
