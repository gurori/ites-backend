namespace ites.DataAccess.Entites
{
    public sealed class TeamEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<Guid> MembersIds { get; set; } = [];
        public Guid? AdminId { get; set; } = null;
        public bool IsPublic { get; set; }
        public string ContentInHtml { get; set; } = string.Empty;
    }
}
