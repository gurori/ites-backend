namespace ites.Infrastructure.Files;

public interface IFileStorage
{
    Task UploadAsync(string path, Stream stream, CancellationToken ct = default);
    Task<(Stream FileStream, string ContentType)> GetAsync(
        string path,
        CancellationToken ct = default
    );
    Task DeleteAsync(string path, CancellationToken ct = default);
}
