using ites.Application.Contracts.Teams;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TeamsController(ITeamService teamService) : BaseController
    {
        private readonly ITeamService _teamService = teamService;

        [HttpPost]
        [HasPermission(Permission.CreateTeam)]
        public async Task<IActionResult> Create(TeamRequest request)
        {
            string token = GetTokenFromCookies();
            await _teamService.CreateAsync(token, request.Name, request.Description);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _teamService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _teamService.GetAsync(id));
        }

        [HttpPut("application/{id:guid}")]
        [HasPermission(Permission.AddTeamAppl)]
        public async Task<IActionResult> AddApplication(Guid id)
        {
            string token = GetTokenFromCookies();
            await _teamService.AddApplicationAsync(token, id);
            return Ok();
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleTeamAppl)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            string token = GetTokenFromCookies();
            await _teamService.HandleApplicationAsync(id, accept);
            return Ok();
        }
    }
}
