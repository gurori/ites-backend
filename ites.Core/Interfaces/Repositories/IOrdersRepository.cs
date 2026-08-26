using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IOrdersRepository : ICrudRepository<Order>
{
    Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Order, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );
    Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
    Task AddOrderBidAsync(OrderBid orderBid, CancellationToken ct = default);

    Task<OrderBid?> GetBidByIdAsync(Guid id, CancellationToken ct = default);
    Task UpdateBidAsync(OrderBid orderBid, CancellationToken ct = default);
}
