using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ites.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController(IOrdersService ordersService) : BaseController
{
    [HttpPost]
    [HasPermission(Permission.CreateOrder)]
    public async Task<IActionResult> Create(
        [FromBody] OrderRequest request,
        CancellationToken ct = default
    )
    {
        var id = await ordersService.CreateAsync(GetUserId(), request, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100,
        CancellationToken ct = default
    )
    {
        var orders = await ordersService.GetAllAsync(page, pageSize, ct);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var order = await ordersService.GetAsync(id, ct);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/bids")]
    [HasPermission(Permission.AddOrderApplication)]
    public async Task<IActionResult> AddBid(
        Guid orderId,
        [FromBody] OrderBidRequest request,
        CancellationToken ct = default
    )
    {
        var id = await ordersService.AddBidAsync(GetUserId(), orderId, request, ct);
        return Ok(id);
    }

    [HttpPatch("bids/{bidId:guid}/handle")]
    [HasPermission(Permission.HandleOrderApplication)]
    public async Task<IActionResult> HandleBid(
        Guid bidId,
        [FromBody] HandleOrderBidRequest request,
        CancellationToken ct = default
    )
    {
        await ordersService.HandleBidAsync(GetUserId(), bidId, request, ct);
        return NoContent();
    }
}
