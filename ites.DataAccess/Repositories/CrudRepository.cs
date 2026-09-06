using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public abstract class CrudRepository<TEntity>(ItesDbContext dbContext)
    : BaseRepository<TEntity>(dbContext),
        ICrudRepository<TEntity>
    where TEntity : BaseEntity
{
    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        int affected = await DbSet.Where(e => e.Id == id).ExecuteDeleteAsync(ct);

        return affected > 0;
    }

    public virtual async Task<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        int skip = 0,
        int take = 100,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .OrderBy(e => e.Id)
            .Select(selector)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(ct);
    }

    public virtual async Task<IReadOnlyCollection<TResult>> GetAllByIdsAsync<TResult>(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(e => ids.Contains(e.Id))
            .Select(selector)
            .ToArrayAsync(ct);
    }

    public virtual async Task<TResult?> GetByIdAsync<TResult>(
        Guid id,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(e => e.Id == id)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        Guid id,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        int skip = 0,
        int take = 100,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .OrderBy(e => e.Id)
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(ct);
    }

    public virtual async Task<ICollection<TEntity>> GetAllByIdsAsync(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, bool>>? predicate = null,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery(predicate, asSplitQuery)
            .Where(e => ids.Contains(e.Id))
            .ToArrayAsync(ct);
    }

    protected IQueryable<T> BuildQuery<T>(Expression<Func<T, bool>>? predicate, bool asSplitQuery)
        where T : BaseEntity
    {
        var query = DbContext.Set<T>().AsQueryable();

        if (asSplitQuery)
            query = query.AsSplitQuery();

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }
}
