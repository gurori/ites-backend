using ites.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.IO.File;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IWebHostEnvironment webHostEnvironment)
        : ControllerBase
    {
        private readonly string _webRootPath = webHostEnvironment.WebRootPath;

        [Authorize]
        [HttpPost("{directory}/{id:guid}")]
        public async Task<IActionResult> Upload(
            string directory, Guid id, [FromForm] Image image)
        {
            try
            {
                Console.WriteLine($"Incoming request: directory={directory}, id={id}, image={image.FormFile.FileName}");
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(ModelState);
                    return BadRequest(ModelState);
                }
                IFormFile file = image.FormFile;
                if (file is null || file.Length == 0)
                    return BadRequest("file is null");

                string uploadFolder = Path
                    .Combine(_webRootPath, "uploads", directory, id.ToString());

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = GetApiFileName(file.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using FileStream stream = new(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{directory}/{id:guid}/{fileName}")]
        public async Task<IActionResult> Get(
            string directory, Guid id, string fileName)
        {
            string rootFolder = Path
                .Combine(_webRootPath, "uploads", directory);
            string folder = Path
                .Combine(rootFolder, id.ToString());

            string contentType = GetContentType(Path
                .GetExtension(fileName));

            if (!Directory.Exists(folder))
            {
                string defualtFolder = Path
                    .Combine(rootFolder, "defualt", fileName);

                if (!Exists(defualtFolder))
                    return NotFound();

                byte[] defualtFileBytes = 
                    await ReadAllBytesAsync(defualtFolder);

                return File(defualtFileBytes, contentType);
            }

            string filePath = Path
                .Combine(folder, fileName);

            byte[] fileBytes = await ReadAllBytesAsync(filePath);

            return File(fileBytes, contentType);
        }

        private static string GetApiFileName(string fileName)
        {
            string fileNameWithoutExtension = Path
                .GetFileNameWithoutExtension(fileName);

            return fileNameWithoutExtension switch
            {
                ApiFileName.Avatar =>
                    $"{fileNameWithoutExtension}.jpg",

                _ => fileName,
            };
        }

        private static string GetContentType(string fileExtension) =>
            fileExtension switch
            {
                ".jpg" => "image/jpeg",
                _ => "application/octet-stream"
            };
    }
    public class Image
    {
        [FromForm(Name = "file")]
        public IFormFile FormFile { get; set; }
    }
}
