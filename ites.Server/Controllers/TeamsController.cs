using ites.Application.Contracts.Teams;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exeptions;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class TeamsController(ITeamService teamService)
        : ControllerBase
    {
        private readonly ITeamService _teamService = teamService;

        [HttpPost("{token}")]
        //[HasPermission(Permission.CreateTeam)]
        public async Task<IActionResult> Create(string token, TeamRequest request)
        {
            try
            {
                await _teamService
                    .CreateAsync(token, request.Name, request.Description);
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _teamService
                .GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _teamService
                    .GetAsync(id));
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }
    }
}
