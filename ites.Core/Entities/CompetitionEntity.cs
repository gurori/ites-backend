namespace ites.Core.Entities
{
    public sealed class CompetitionEntity
    {
        public Guid Id { get; set; }
        public string ContentInHtml { get; set; } = string.Empty;
        public IList<Guid> MembersIds { get; set; } = [];
        public IList<Guid> OrganizersIds { get; set; } = [];
    }
}
