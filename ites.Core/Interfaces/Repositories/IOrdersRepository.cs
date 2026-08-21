using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IOrdersRepository : IRepository<Order>
{
    public Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Order, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );
    public Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
}
