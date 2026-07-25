using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrdersController(IOrdersService ordersService) : BaseController
    {
        private readonly IOrdersService _ordersService = ordersService;

        [HttpPost]
        [HasPermission(Permission.CreateOrd)]
        public async Task<IActionResult> Create(OrderRequest request)
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _ordersService.GetAsync());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _ordersService.GetAsync(id));
        }

        [HttpGet("many")]
        public async Task<IActionResult> Get([FromQuery] IList<Guid> ids)
        {
            return Ok(await _ordersService.GetAsync(ids));
        }

        [HttpPut("application/{id:guid}")]
        [HasPermission(Permission.AddOrdAppl)]
        public async Task<IActionResult> AddApplication(Guid id)
        {
            string token = GetTokenFromHeaders();
            await _ordersService.AddApplicationAsync(token, id);
            return Ok();
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleOrdAppl)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            string token = GetTokenFromHeaders();
            await _ordersService.HandleApplicationAsync(id, accept);
            return Ok();
        }
    }
}
