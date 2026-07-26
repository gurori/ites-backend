using ites.Application.Contracts.Competitions;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Models;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CompetitionsController(ICompetitionsService competitionsService)
        : BaseController
    {
        private readonly ICompetitionsService _competitionsService = competitionsService;

        [HttpPost("create")]
        [HasPermission(Permission.CreateCmp)]
        public async Task<IActionResult> Create(CompetitionRequest request)
        {
            Guid userId = GetUserIdFromJwt();
            await _competitionsService.CreateAsync(userId, request.ContentInHtml);
            return Ok();
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _competitionsService.GetAsync());
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Competition competition = await _competitionsService.GetAsync(id);
            return Ok(competition);
        }

        [HttpGet("get/many")]
        public async Task<IActionResult> Get([FromQuery] IList<Guid> ids)
        {
            IList<Competition> competitions = await _competitionsService.GetAsync(ids);
            return Ok(competitions);
        }

        [HttpPut("application/{id:guid}")]
        [HasPermission(Permission.AddCmpAppl)]
        public async Task<IActionResult> AddAppication(Guid id)
        {
            Guid userId = GetUserIdFromJwt();
            await _competitionsService.AddApplicationAsync(userId, id);
            return Ok();
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleCmpAppl)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            await _competitionsService.HandleApplicationAsync(id, accept);
            return Ok();
        }
    }
}
