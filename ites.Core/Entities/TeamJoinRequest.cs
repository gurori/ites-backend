namespace ites.Core.Entities;

public sealed class TeamJoinRequest : BaseRequestEntity
{
    public Guid TeamId { get; set; }
    public Team Team { get; set; } = null!;
}
