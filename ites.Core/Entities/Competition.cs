namespace ites.Core.Entities
{
    public sealed class Competition : BaseEntity
    {
        public string ContentInHtml { get; set; } = string.Empty;
        public ICollection<User> Members { get; set; } = [];
        public ICollection<User> Organizers { get; set; } = [];
    }
}
