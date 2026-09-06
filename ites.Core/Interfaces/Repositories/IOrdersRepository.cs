using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IOrdersRepository : ICrudRepository<Order>
{
    Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
    Task AddOrderBidAsync(OrderBid orderBid, CancellationToken ct = default);

    Task<TResult?> GetBidByIdAsync<TResult>(
        Guid id,
        Expression<Func<OrderBid, TResult>> selector,
        Expression<Func<OrderBid, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<OrderBid?> GetBidByIdAsync(
        Guid id,
        Expression<Func<OrderBid, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task UpdateBidAsync(OrderBid orderBid, CancellationToken ct = default);
}
