namespace ites.Core.Entities;

public sealed class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }

    public ICollection<User> Members { get; set; } = [];
    public Guid AdminId { get; set; }
    public User Admin { get; set; } = null!;
    public ICollection<TeamJoinRequest> JoinRequests { get; set; } = [];
}
