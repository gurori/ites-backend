using ites.Application.Services;
using ites.Core.Enums;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HasPermission(Permission.Moderate)]
    public sealed class ModersController(ModersService modersService) : ControllerBase
    {
        private readonly ModersService _modersService = modersService;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _modersService.GetAllAsync());
        }

        [HttpPost("{type}/{id:guid}/{accept:bool}")]
        public async Task<IActionResult> Handle(string type, Guid id, bool accept)
        {
            if (type != "team" && type != "order")
            {
                return BadRequest();
            }
            await _modersService.HandleAsync(type, id, accept);
            return Ok();
        }
    }
}
