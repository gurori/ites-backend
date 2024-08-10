namespace ites.DataAccess.Entites
{
    public sealed class CompetitionEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<Guid> MembersIds { get; set; } = [];
        public IList<Guid> OrganizersIds { get; set; } = [];
    }
}
