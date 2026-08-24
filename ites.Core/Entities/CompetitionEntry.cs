namespace ites.Core.Entities;

public sealed class CompetitionEntry : BaseRequestEntity
{
    public Guid CompetitionId { get; set; }
    public Competition Competition { get; set; } = null!;
}
