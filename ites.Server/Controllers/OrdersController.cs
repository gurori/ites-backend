using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class OrdersController(IOrdersService ordersService) : BaseController
    {
        private readonly IOrdersService _ordersService = ordersService;

        [HttpPost]
        [HasPermission(Permission.CreateOrder)]
        public async Task<IActionResult> Create(OrderRequest request)
        {
            Guid userId = GetUserId();
            await _ordersService.CreateAsync(
                userId,
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
        [HasPermission(Permission.AddOrderApplication)]
        public async Task<IActionResult> AddApplication(Guid id)
        {
            Guid userId = GetUserId();
            await _ordersService.AddApplicationAsync(userId, id);
            return Ok();
        }

        [HttpPut("application/{id:guid}/{accept:bool}")]
        [HasPermission(Permission.HandleOrderApplication)]
        public async Task<IActionResult> HandleApplication(Guid id, bool accept)
        {
            await _ordersService.HandleApplicationAsync(id, accept);
            return Ok();
        }
    }
}
