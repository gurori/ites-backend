using ites.Core.Enums;

namespace ites.Core.Entities;

public abstract class BaseRequestEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public RequsetStatus Status { get; set; } = RequsetStatus.Pending;
    public string CoverLetter { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
