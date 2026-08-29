namespace ites.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> UploadUserAvatarAsync(
        Guid userId,
        string fileName,
        Stream stream,
        CancellationToken ct = default
    );
    Task<string> UploadCompetitionImageAsync(
        Guid userId,
        Guid competitionId,
        string fileName,
        Stream stream,
        CancellationToken ct = default
    );

    Task<(Stream FileStream, string ContentType)> GetAsync(
        string path,
        CancellationToken ct = default
    );
    Task<(Stream FileStream, string ContentType)> GetUserAvatarAsync(
        Guid userId,
        CancellationToken ct = default
    );
}
