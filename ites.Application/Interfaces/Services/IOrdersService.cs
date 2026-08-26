using ites.Application.Contracts.Orders;

namespace ites.Application.Interfaces.Services;

public interface IOrdersService
{
    Task<Guid> CreateAsync(Guid userId, OrderRequest request, CancellationToken ct = default);
    Task<OrderResponse> GetAsync(Guid id, CancellationToken ct = default);
    Task<OrderListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    );
    Task<Guid> AddBidAsync(
        Guid userId,
        Guid orderId,
        OrderBidRequest request,
        CancellationToken ct = default
    );
    Task HandleBidAsync(
        Guid userId,
        Guid bidId,
        HandleOrderBidRequest request,
        CancellationToken ct = default
    );
    Task UpdateAsync(
        Guid userId,
        Guid orderId,
        UpdateOrderRequest request,
        CancellationToken ct = default
    );
    Task DeleteAsync(Guid id, Guid clientId, CancellationToken ct = default);
}
