namespace ites.Core.Entities;

public sealed class RequestEntity : BaseEntity
{
    public Guid For { get; set; }
    public Guid From { get; set; }
}
