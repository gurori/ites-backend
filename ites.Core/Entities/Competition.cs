namespace ites.Core.Entities
{
    public sealed class Competition : BaseEntity
    {
        public string ContentInHtml { get; set; } = string.Empty;
        public ICollection<Guid> MembersIds { get; set; } = [];
        public ICollection<Guid> OrganizersIds { get; set; } = [];
    }
}
