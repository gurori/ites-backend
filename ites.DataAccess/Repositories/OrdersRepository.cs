using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class OrdersRepository(ItesDbContext context)
    : CrudRepository<Order>(context),
        IOrdersRepository
{
    public Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default)
    {
        return DbSet
            .Where(o => o.Id == id)
            .ExecuteUpdateAsync(o => o.SetProperty(x => x.IsPublic, isPublic), ct);
    }

    public async Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Order, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    )
    {
        IReadOnlyCollection<T> orders = await DbSet
            .Where(o => o.IsPublic == isPublic)
            .Select(selector)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);

        return orders;
    }

    public Task AddOrderBidAsync(OrderBid orderBid, CancellationToken ct = default)
    {
        DbContext.OrderBids.Add(orderBid);
        return Task.CompletedTask;
    }

    public async Task<OrderBid?> GetBidByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await DbContext.OrderBids.FindAsync([id], ct);
    }

    public Task UpdateBidAsync(OrderBid orderBid, CancellationToken ct = default)
    {
        DbContext.OrderBids.Update(orderBid);
        return Task.CompletedTask;
    }
}
