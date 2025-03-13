using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exeptions;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrdersController(IOrdersService ordersService) : ControllerBase
    {
        private readonly IOrdersService _ordersService = ordersService;

        [HttpPost]
        [HasPermission(Permission.CreateOrd)]
        public async Task<IActionResult> Create(OrderRequest request)
        {
            try
            {
                string token = GetTokenFromHeaders();
                await _ordersService.CreateAsync(
                    token,
                    request.Title,
                    request.Description,
                    request.Price,
                    request.DeadLine
                );
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
            return Ok(await _ordersService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _ordersService.GetAsync(id));
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpGet("many")]
        public async Task<IActionResult> Get([FromQuery] IList<Guid> ids)
        {
            try
            {
                return Ok(await _ordersService.GetAsync(ids));
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpPut("application/{id:guid}")]
        [HasPermission(Permission.AddOrdAppl)]
        public async Task<IActionResult> AddApplication(Guid id)
        {
            try
            {
                string token = GetTokenFromHeaders();
                await _ordersService.AddApplicationAsync(token, id);
                return Ok();
            }
            catch (ApiException ex)
            {
                return Problem(detail: ex.Message, statusCode: ex.StatusCode);
            }
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleOrdAppl)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            try
            {
                string token = GetTokenFromHeaders();
                await _ordersService.HandleApplicationAsync(id, accept);
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
