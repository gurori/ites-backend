using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications;

public sealed record OrderBidResponse(
    Guid Id,
    MemberSummaryResponse FromMember,
    OrderSummaryResponse ForOrder
);
