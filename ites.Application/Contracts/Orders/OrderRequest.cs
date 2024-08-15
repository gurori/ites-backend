namespace ites.Application.Contracts.Orders
{
    public record OrderRequest(
        string Title,
        string Description,
        decimal Price,
        DateTime DeadLine
        );
}
