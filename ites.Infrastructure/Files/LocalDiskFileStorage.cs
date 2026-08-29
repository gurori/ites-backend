using ites.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace ites.Infrastructure.Files;

public sealed class LocalDiskFileStorage(IWebHostEnvironment environment) : IFileStorage
{
    private readonly string _uploadsDirectory = Path.Combine(environment.WebRootPath, "uploads");
    private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

    public async Task UploadAsync(string path, Stream stream, CancellationToken ct = default)
    {
        string uploadPath = GetSecurePath(path);

        string? directoryPath = Path.GetDirectoryName(uploadPath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        await using FileStream fileStream = new(
            uploadPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None
        );

        await stream.CopyToAsync(fileStream, ct);
    }

    public Task<(Stream FileStream, string ContentType)> GetAsync(
        string path,
        CancellationToken ct = default
    )
    {
        string uploadPath = GetSecurePath(path);

        if (!File.Exists(uploadPath))
            throw new NotFoundException($"File {path} not found.");

        if (!_contentTypeProvider.TryGetContentType(uploadPath, out string? contentType))
        {
            contentType = "application/octet-stream";
        }

        Stream stream = File.OpenRead(uploadPath);

        return Task.FromResult((stream, contentType));
    }

    public Task DeleteAsync(string path, CancellationToken ct = default)
    {
        string uploadPath = GetSecurePath(path);

        if (File.Exists(uploadPath))
        {
            File.Delete(uploadPath);
        }

        return Task.CompletedTask;
    }

    private string GetSecurePath(string path)
    {
        string uploadPath = Path.GetFullPath(Path.Combine(_uploadsDirectory, path));

        if (
            !uploadPath.StartsWith(
                Path.GetFullPath(_uploadsDirectory),
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            throw new InvalidOperationException("Попытка выхода за пределы базовой директории.");
        }

        return uploadPath;
    }
}
