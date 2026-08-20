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
    public ICollection<Guid> MembersIds { get; private set; } = [];
    public ICollection<Guid> OrganizersIds { get; private set; } = [];
}
