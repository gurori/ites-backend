using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts
{
    public record CompetitionApplicationResponse(
        Guid Id,
        UserProfileResponse FromMember,
        CompetitionResponse ForCompetition
        );
}
