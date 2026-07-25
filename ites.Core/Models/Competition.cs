namespace ites.Core.Models
{
    public sealed class Competition
    {
        public Competition(Guid id, string contentInHtml)
        {
            Id = id;
            ContentInHtml = contentInHtml;
            MembersIds = [];
            OrganizersIds = [];
        }

        public Competition() { }

        public Guid Id { get; private set; }
        public string ContentInHtml { get; private set; }

        // public string Title { get; private set; } = title;
        // public string Description { get; private set; } = description;
        // public DateTime StartDate { get; private set; } = startDate;
        // public DateTime EndDate { get; private set; } = endDate;
        public IList<Guid> MembersIds { get; private set; }
        public IList<Guid> OrganizersIds { get; private set; }
    }
}
