namespace ites.Core.Entities
{
    public sealed class OrderEntity : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime DeadLine { get; set; }
        public Guid ClientId { get; set; }
        public Guid MemberId { get; set; }
        public bool IsPublic { get; set; }
        public string ContentInHtml { get; set; } = string.Empty;
    }
}
