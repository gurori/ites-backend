using ites.Application.Contracts.Orders;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Enums;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class OrdersService(IOrdersRepository ordersRepository) : IOrdersService
{
    public async Task<Guid> AddBidAsync(
        Guid userId,
        Guid orderId,
        OrderBidRequest request,
        CancellationToken ct = default
    )
    {
        var order =
            await ordersRepository.GetByIdAsync(orderId, o => new { o.IsPublic }, ct)
            ?? throw new NotFoundException("Заказ не найден.");

        if (!order.IsPublic)
            throw new BadRequestException("Этот заказ больше не принимает отклики.");

        var bid = new OrderBid
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            OrderId = orderId,
            CoverLetter = request.CoverLetter ?? string.Empty,
            ProposedPrice = request.ProposedPrice,
            Status = RequsetStatus.Pending,
        };

        await ordersRepository.AddOrderBidAsync(bid, ct);
        await ordersRepository.SaveChangesAsync(ct);
        return bid.Id;
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

    public async Task HandleBidAsync(
        Guid userId,
        Guid bidId,
        HandleOrderBidRequest request,
        CancellationToken ct = default
    )
    {
        var bid =
            await ordersRepository.GetBidByIdAsync(bidId, ct)
            ?? throw new NotFoundException("Отклик не найден.");

        if (bid.Status != RequsetStatus.Pending)
            throw new BadRequestException("Эта заявка уже обработана.");

        var order =
            await ordersRepository.GetByIdAsync(bid.OrderId, ct)
            ?? throw new NotFoundException("Заказ не найден.");

        if (order.ClientId != userId)
            throw new ForbiddenException("У вас нет прав для обработки заявок этого заказа.");

        if (request.Accept)
        {
            bid.Status = RequsetStatus.Accepted;

            order.MemberId = bid.UserId;
            order.IsPublic = false;

            await ordersRepository.UpdateAsync(order, ct);
        }
        else
        {
            bid.Status = RequsetStatus.Rejected;
        }

        await ordersRepository.UpdateBidAsync(bid, ct);
        await ordersRepository.SaveChangesAsync(ct);
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
