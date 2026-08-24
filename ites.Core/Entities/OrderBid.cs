using ites.Core.Enums;

namespace ites.Core.Entities;

public sealed class OrderBid : BaseRequestEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public decimal? ProposedPrice { get; set; }
}
