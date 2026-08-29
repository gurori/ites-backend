namespace ites.Core.Entities;

public sealed class FileEntity : BaseEntity
{
    public string Directory { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }

    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
}
