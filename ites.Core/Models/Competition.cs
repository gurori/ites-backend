namespace ites.Core.Models
{
    public sealed class Competition(
        Guid id,
        string contentInHtml)
    {
        public Guid Id { get; private set; } = id;
        public string ContentInHtml { get; private set; } = contentInHtml;
        // public string Title { get; private set; } = title;
        // public string Description { get; private set; } = description;
        // public DateTime StartDate { get; private set; } = startDate;
        // public DateTime EndDate { get; private set; } = endDate;
        public IList<Guid> MembersIds { get; private set; }
        public IList<Guid> OrganizersIds { get; private set; }
    }
}
