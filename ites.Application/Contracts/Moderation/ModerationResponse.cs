using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;

namespace ites.Application.Contracts.Moderation;

public sealed record ModerationResponse(
    IReadOnlyCollection<TeamResponse> Teams,
    IReadOnlyCollection<OrderResponse> Orders
);
