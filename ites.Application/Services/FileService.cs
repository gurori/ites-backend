using ites.Application.Constants;
using ites.Application.Interfaces.Files;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class FileService(
    IFileStorage fileStorage,
    IFileEntityRepository fileEntityRepository,
    ICompetitionsRepository competitionsRepository,
    IUserRepository userRepository
) : IFileService
{
    public Task<(Stream FileStream, string ContentType)> GetAsync(
        string path,
        CancellationToken ct = default
    )
    {
        return fileStorage.GetAsync(path, ct);
    }

    public async Task<(Stream FileStream, string ContentType)> GetUserAvatarAsync(
        Guid userId,
        CancellationToken ct = default
    )
    {
        var user =
            await userRepository.GetByIdAsync(userId, u => new { u.AvatarPath }, ct: ct)
            ?? throw new NotFoundException("Пользователь не найден.");

        var path = user.AvatarPath ?? FileConstants.DefaultAvatarStoragePath;

        return await fileStorage.GetAsync(path, ct);
    }

    public async Task<string> UploadCompetitionImageAsync(
        Guid userId,
        Guid competitionId,
        string fileName,
        Stream stream,
        CancellationToken ct = default
    )
    {
        var competition =
            await competitionsRepository.GetByIdAsync(
                competitionId,
                c => new { OrganizerIds = c.Organizers.Select(o => o.Id) },
                ct: ct
            ) ?? throw new NotFoundException("Конкурс не найден.");

        if (!competition.OrganizerIds.Contains(userId))
            throw new ForbiddenException("Пользователь не является организатором данного конкурса");

        var fileId = Guid.CreateVersion7();
        var extension = Path.GetExtension(fileName);
        var safeFileName = $"{fileId}{extension}";
        var storagePath = $"competitions/{competitionId}/{safeFileName}";

        await fileStorage.UploadAsync(storagePath, stream, ct);

        var fileEntity = new FileEntity
        {
            Id = fileId,
            Directory = $"competitions/{competitionId}",
            FileName = safeFileName,
            ContentType = GetContentType(extension),
            UploadedById = userId,
            UploadedAt = DateTime.UtcNow,
            SizeInBytes = stream.Length,
        };

        try
        {
            await fileEntityRepository.CreateAsync(fileEntity, ct);
        }
        catch
        {
            await fileStorage.DeleteAsync(storagePath, ct);
            throw;
        }

        return $"/api/files/{storagePath}";
    }

    public async Task<string> UploadUserAvatarAsync(
        Guid userId,
        string fileName,
        Stream stream,
        CancellationToken ct = default
    )
    {
        var user =
            await userRepository.GetByIdAsync(userId, ct: ct)
            ?? throw new NotFoundException("Пользователь не найден.");

        var extension = Path.GetExtension(fileName).ToLower();
        var storagePath = $"users/{userId}/avatar{extension}";

        await fileStorage.UploadAsync(storagePath, stream, ct);

        user.AvatarPath = storagePath;
        await userRepository.UpdateAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        return $"/api/files/{storagePath}";
    }

    private static string GetContentType(string extension) =>
        extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };
}
