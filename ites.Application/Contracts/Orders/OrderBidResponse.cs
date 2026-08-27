using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Orders;

public sealed record OrderBidResponse(
    Guid Id,
    MemberSummaryResponse FromMember,
    OrderSummaryResponse ForOrder
);
