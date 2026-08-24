using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class OrdersService(
    IOrdersRepository ordersRepository,
    IRequestEntityRepository applicationsRepository
) : IOrdersService
{
    public async Task AddApplicationAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        RequestEntity application = new()
        {
            Id = Guid.CreateVersion7(),
            For = orderId,
            From = userId,
        };

        await applicationsRepository.CreateForOrderAsync(application, ct);
        await ordersRepository.SaveChangesAsync(ct);
    }

    public async Task<Guid> CreateAsync(
        Guid userId,
        OrderRequest request,
        CancellationToken ct = default
    )
    {
        Order order = new()
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            DeadLine = request.DeadLine,
            ClientId = userId,
        };

        await ordersRepository.CreateAsync(order, ct);
        await ordersRepository.SaveChangesAsync(ct);
        return order.Id;
    }

    public Task DeleteAsync(Guid id, Guid clientId, CancellationToken ct = default)
    {
        // TODO: Implement the logic to delete an order by its ID
        throw new NotImplementedException();
    }

    public async Task<OrderListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var orders = await ordersRepository.GetAllAsync(
            o => new OrderSummaryResponse(o.Id, o.Title, o.Description, o.Price, o.DeadLine),
            (page - 1) * pageSize,
            pageSize,
            ct
        );
        return new OrderListResponse(orders, await ordersRepository.CountAsync(ct), page, pageSize);
    }

    public async Task<OrderResponse> GetAsync(Guid id, CancellationToken ct = default)
    {
        var order =
            await ordersRepository.GetByIdAsync(
                id,
                o => new OrderResponse(o.Id, o.Title, o.Description, o.Price, o.DeadLine),
                ct
            ) ?? throw new NotFoundException("Заказ не найден");

        return order;
    }

    public Task HandleApplicationAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        return applicationsRepository.HandleOrderAsync(id, isAccept, ct);
    }

    public Task UpdateAsync(
        Guid userId,
        Guid orderId,
        UpdateOrderRequest request,
        CancellationToken ct = default
    )
    {
        // TODO: Implement the logic to update an order by its ID and user ID
        throw new NotImplementedException();
    }
}
