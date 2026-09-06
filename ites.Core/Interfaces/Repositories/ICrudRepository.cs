using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ICrudRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TResult?> GetByIdAsync<TResult>(
        Guid id,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<TEntity?> GetByIdAsync(
        Guid id,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        int skip = 0,
        int take = 100,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        int skip = 0,
        int take = 100,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<IReadOnlyCollection<TResult>> GetAllByIdsAsync<TResult>(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task<ICollection<TEntity>> GetAllByIdsAsync(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );

    Task UpdateAsync(TEntity entity, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
