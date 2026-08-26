using ites.Infrastructure.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController(IFileService fileService) : ControllerBase
{
    [Authorize]
    [HttpPost("{directory}/{id:guid}")]
    public async Task<IActionResult> Upload(string directory, Guid id, [FromForm] Image image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (image.FormFile is null || image.FormFile.Length == 0)
            return BadRequest("File is required.");

        await fileService.UploadAsync(
            directory,
            id,
            image.FormFile.OpenReadStream(),
            image.FormFile.FileName
        );

        return NoContent();
    }

    [HttpGet("{directory}/{id:guid}/{fileName}")]
    public async Task<IActionResult> Get(string directory, Guid id, string fileName)
    {
        var (file, contentType) = await fileService.GetAsync(directory, id, fileName);

        return File(file, contentType);
    }
}

public sealed class Image
{
    [FromForm(Name = "file")]
    public IFormFile? FormFile { get; set; }
}
