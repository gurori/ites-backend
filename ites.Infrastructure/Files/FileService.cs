using ites.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace ites.Infrastructure.Files;

public sealed class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly string _webRootPath = environment.WebRootPath;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    public async Task UploadAsync(string directory, Guid id, Stream stream, string fileName)
    {
        directory = Path.GetFileName(directory);
        fileName = Path.GetFileName(fileName);

        string uploadFolder = Path.Combine(_webRootPath, "uploads", directory, id.ToString());

        Directory.CreateDirectory(uploadFolder);

        string apiFileName = GetApiFileName(fileName);
        string filePath = Path.Combine(uploadFolder, apiFileName);

        await using FileStream fileStream = new(
            filePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None
        );

        await stream.CopyToAsync(fileStream);
    }

    public Task<(Stream FileStream, string ContentType)> GetAsync(
        string directory,
        Guid id,
        string fileName
    )
    {
        fileName = Path.GetFileName(fileName);
        directory = Path.GetFileName(directory);

        string rootFolder = Path.Combine(_webRootPath, "uploads", directory);
        string folder = Path.Combine(rootFolder, id.ToString());

        string filePath = Directory.Exists(folder)
            ? Path.Combine(folder, fileName)
            : Path.Combine(rootFolder, "default", fileName);

        if (!File.Exists(filePath))
            throw new NotFoundException($"File {fileName} not found.");

        if (!_contentTypeProvider.TryGetContentType(filePath, out string? contentType))
        {
            contentType = "application/octet-stream";
        }

        Stream stream = File.OpenRead(filePath);

        return Task.FromResult((stream, contentType));
    }

    private static string GetApiFileName(string fileName)
    {
        string name = Path.GetFileNameWithoutExtension(fileName);

        return name switch
        {
            FileNames.UserAvatar => $"{name}.jpg",
            _ => fileName,
        };
    }
}
