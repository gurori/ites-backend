namespace ites.Core.Models;

public sealed class Competition
{
    public Competition(Guid id, string contentInHtml)
    {
        Id = id;
        ContentInHtml = contentInHtml;
    }

    public Competition() { }

    public Guid Id { get; private set; }
    public string ContentInHtml { get; private set; } = string.Empty;
    public IList<Guid> MembersIds { get; private set; } = [];
    public IList<Guid> OrganizersIds { get; private set; } = [];
}
