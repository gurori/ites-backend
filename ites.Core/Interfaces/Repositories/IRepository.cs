using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<bool> CreateAsync(TEntity entity, CancellationToken ct = default);

    Task<T?> GetByIdAsync<T>(
        Guid id,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    );

    Task<IReadOnlyCollection<T>> GetAllAsync<T>(
        Expression<Func<TEntity, T>> selector,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );

    Task<IReadOnlyCollection<T>> GetAllByIdsAsync<T>(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    );

    Task UpdateAsync(TEntity entity, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
