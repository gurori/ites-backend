namespace ites.Application.Contracts.Orders;

public sealed record OrderSummaryResponse(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    DateTime DeadLine
);
