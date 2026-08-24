namespace ites.Application.Contracts.Orders;

public sealed record UpdateOrderRequest(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    DateTime DeadLine
);
