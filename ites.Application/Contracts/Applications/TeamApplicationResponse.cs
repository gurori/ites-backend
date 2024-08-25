using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications
{
    public record TeamApplicationResponse(
        Guid Id,
        UserProfileResponse FromMember
        );
}
