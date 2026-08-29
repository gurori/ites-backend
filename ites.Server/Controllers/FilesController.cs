using System.Collections.Frozen;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exceptions;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class FilesController(IFileService fileService) : BaseController
{
    private static readonly FrozenSet<string> _allowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp",
        ".gif",
    ];

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar(
        [FromForm(Name = "file")] IFormFile image,
        CancellationToken ct = default
    )
    {
        CheckFile(image);

        await using var stream = image.OpenReadStream();
        string url = await fileService.UploadUserAvatarAsync(
            GetUserId(),
            image.FileName,
            stream,
            ct
        );

        return Ok(new { url });
    }

    [HasPermission(Permission.CreateCompetition)]
    [HttpPost("competition/{id:guid}")]
    public async Task<IActionResult> UploadCompetitionImage(
        Guid id,
        [FromForm(Name = "file")] IFormFile image,
        CancellationToken ct = default
    )
    {
        CheckFile(image);

        await using var stream = image.OpenReadStream();
        string url = await fileService.UploadCompetitionImageAsync(
            GetUserId(),
            id,
            image.FileName,
            stream,
            ct
        );

        return Ok(new { url });
    }

    [AllowAnonymous]
    [HttpGet("{*path}")]
    public async Task<IActionResult> Get(string path, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(path))
            return BadRequest("Путь к файлу не указан.");

        var (file, contentType) = await fileService.GetAsync(path, ct);

        return File(file, contentType);
    }

    private void CheckFile(IFormFile file)
    {
        if (file is null || file.Length <= 0)
            throw new BadRequestException("Файл не загружен или пуст.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(extension))
            throw new BadRequestException("Неподдерживаемый формат файла.");
    }
}
