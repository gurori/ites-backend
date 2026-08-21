namespace ites.Core.Entities
{
    public sealed class Team : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<User> Members { get; set; } = [];
        public Guid AdminId { get; set; }
        public User Admin { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}
