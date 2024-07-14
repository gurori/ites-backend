using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController(IWebHostEnvironment webHostEnvironment) 
        : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [HttpPost("{directory}/{id:guid}")]
        public async Task<IActionResult> Upload(
            string directory, Guid id, IFormFile file)
        {
            if(file is null || file.Length == 0) 
                return BadRequest();

            string uploadFolder = Path
                .Combine(_webHostEnvironment.WebRootPath, "uploads/", directory, id.ToString());

            if(!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string filePath = Path.Combine(uploadFolder, file.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Ok();
        }
    }
}
