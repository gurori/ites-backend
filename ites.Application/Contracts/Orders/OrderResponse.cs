namespace ites.Application.Contracts.Orders
{
    public record OrderResponse(
        Guid Id,
        string Title,
        string Description,
        decimal Price,
        DateTime DeadLine
    );
}
