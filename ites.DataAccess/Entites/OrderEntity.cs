namespace ites.DataAccess.Entites
{
    public sealed class OrderEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime DeadLine { get; set; }
        public Guid ClientId { get; set; }
        public Guid MemberId { get; set; }
    }
}
