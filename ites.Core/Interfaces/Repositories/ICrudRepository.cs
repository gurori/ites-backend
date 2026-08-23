using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ICrudRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<T?> GetByIdAsync<T>(
        Guid id,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    );

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyCollection<T>> GetAllAsync<T>(
        Expression<Func<TEntity, T>> selector,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );

    Task<ICollection<TEntity>> GetAllAsync(
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );

    Task<IReadOnlyCollection<T>> GetAllByIdsAsync<T>(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    );

    Task<ICollection<TEntity>> GetAllByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken ct = default
    );

    Task UpdateAsync(TEntity entity, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
