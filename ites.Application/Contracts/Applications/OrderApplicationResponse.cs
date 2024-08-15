using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications
{
    public record OrderApplicationResponse(
        Guid Id,
        UserProfileResponse FromMember,
        OrderResponse ForOrder
        );
}
