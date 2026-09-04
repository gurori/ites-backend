namespace ites.Application.Contracts.Orders;

public sealed record OrderWithBidsResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    DateTime Deadline,
    IReadOnlyCollection<OrderBidResponse> Bids
);
