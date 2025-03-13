using ites.Application.Contracts.Competitions;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exeptions;
using ites.Core.Models;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CompetitionsController(ICompetitionsService competitionsService)
        : ControllerBase
    {
        private readonly ICompetitionsService _competitionsService = competitionsService;

        [HttpPost("create")]
        [HasPermission(Permission.CreateCmp)]
        public async Task<IActionResult> Create(CompetitionRequest request)
        {
            try
            {
                string token = GetTokenFromHeaders();
                await _competitionsService.CreateAsync(
                    token,
                    request.Title,
                    request.Description,
                    request.StartDate,
                    request.EndDate
                );
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _competitionsService.GetAsync());
        }

        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                Competition competition = await _competitionsService.GetAsync(id);
                return Ok(competition);
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
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
            try
            {
                string token = GetTokenFromHeaders();
                await _competitionsService.AddApplicationAsync(token, id);
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleCmpAppl)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            try
            {
                await _competitionsService.HandleApplicationAsync(id, accept);
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        private string GetTokenFromHeaders() =>
            Request.Headers.Authorization.FirstOrDefault()!.Split(" ").Last();
    }
}
