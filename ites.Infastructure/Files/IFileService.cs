using Microsoft.AspNetCore.Http;

namespace ites.Infastructure.Files;

public interface IFileService
{
    public Task UploadAsync(string directory, Guid id, IFormFile file);

    public Task<(Stream FileStream, string ContentType)> GetAsync(
        string directory,
        Guid id,
        string fileName
    );
}
