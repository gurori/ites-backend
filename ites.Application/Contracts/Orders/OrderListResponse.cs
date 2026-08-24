namespace ites.Application.Contracts.Orders;

public sealed record OrderListResponse(
    IReadOnlyCollection<OrderSummaryResponse> Orders,
    int TotalCount,
    int Page,
    int PageSize
);
