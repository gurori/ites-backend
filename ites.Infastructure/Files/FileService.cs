using System.IO;
using ites.Core.Exeptions;
using ites.Core.Structs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ites.Infastructure.Files;

public sealed class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly string _webRootPath = environment.WebRootPath;

    public async Task UploadAsync(string directory, Guid id, IFormFile file)
    {
        string uploadFolder = Path.Combine(_webRootPath, "uploads", directory, id.ToString());

        Directory.CreateDirectory(uploadFolder);

        string fileName = GetApiFileName(file.FileName);
        string filePath = Path.Combine(uploadFolder, fileName);

        await using FileStream stream = new(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public async Task<(Stream FileStream, string ContentType)> GetAsync(
        string directory,
        Guid id,
        string fileName
    )
    {
        string rootFolder = Path.Combine(_webRootPath, "uploads", directory);
        string folder = Path.Combine(rootFolder, id.ToString());

        string contentType = GetContentType(Path.GetExtension(fileName));
        string filePath;

        if (!Directory.Exists(folder))
        {
            filePath = Path.Combine(rootFolder, "default", fileName);

            if (!File.Exists(filePath))
                throw new NotFoundException($"File {fileName} not found.");
        }
        else
        {
            filePath = Path.Combine(folder, fileName);
        }

        if (!File.Exists(filePath))
            throw new NotFoundException($"File {fileName} not found.");

        Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        return (stream, contentType);
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

    private static string GetContentType(string extension) =>
        extension switch
        {
            ".jpg" => "image/jpeg",
            _ => "application/octet-stream",
        };
}
