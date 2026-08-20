namespace ites.Core.Entities
{
    public sealed class TeamEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<Guid> MembersIds { get; set; } = [];
        public Guid? AdminId { get; set; } = null;
        public bool IsPublic { get; set; }
        public string ContentInHtml { get; set; } = string.Empty;
    }
}
