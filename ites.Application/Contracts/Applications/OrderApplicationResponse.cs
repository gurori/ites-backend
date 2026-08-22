using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications;

public sealed record OrderApplicationResponse(
    Guid Id,
    MemberSummaryResponse FromMember,
    OrderResponse ForOrder
);
