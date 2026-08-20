namespace ites.Application.Contracts.Orders;

public sealed record OrderRequest(
    string Title,
    string Description,
    decimal Price,
    DateTime DeadLine
);
