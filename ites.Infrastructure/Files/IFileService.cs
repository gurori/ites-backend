namespace ites.Infrastructure.Files;

public interface IFileService
{
    public Task UploadAsync(string directory, Guid id, Stream stream, string fileName);

    public Task<(Stream FileStream, string ContentType)> GetAsync(
        string directory,
        Guid id,
        string fileName
    );
}
