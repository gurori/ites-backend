namespace ites.Core.Models
{
    public sealed class Order(
        Guid id,
        string title,
        string description,
        decimal price,
        DateTime deadLine
        )
    {
        public Guid Id { get; private set; } = id;
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public decimal Price { get; private set; } = price;
        public DateTime DeadLine { get; private set; } = deadLine;
        public Guid ClientId { get; private set; }
        public Guid MemberId { get; private set; }
    }
}
