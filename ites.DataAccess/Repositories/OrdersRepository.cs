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

    public Task AddOrderBidAsync(OrderBid orderBid, CancellationToken ct = default)
    {
        DbContext.OrderBids.Add(orderBid);
        return Task.CompletedTask;
    }

    public async Task<OrderBid?> GetBidByIdAsync(
        Guid id,
        Expression<Func<OrderBid, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public Task UpdateBidAsync(OrderBid orderBid, CancellationToken ct = default)
    {
        DbContext.OrderBids.Update(orderBid);
        return Task.CompletedTask;
    }

    public async Task<TResult?> GetBidByIdAsync<TResult>(
        Guid id,
        Expression<Func<OrderBid, TResult>> selector,
        Expression<Func<OrderBid, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(b => b.Id == id)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    }
}
