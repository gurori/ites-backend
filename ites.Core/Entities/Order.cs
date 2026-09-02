namespace ites.Core.Entities;

public sealed class Order : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime DeadLine { get; set; }
    public bool IsPublic { get; set; }
    public string ContentInHtml { get; set; } = string.Empty;

    public User Client { get; set; } = null!;
    public Guid ClientId { get; set; }
    public User? Member { get; set; } = null;
    public Guid? MemberId { get; set; } = null;
    public ICollection<OrderBid> Bids { get; set; } = [];
}
