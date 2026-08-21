using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public abstract class BaseRepository<TEntity>(ItesDbContext dbContext) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ItesDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public virtual Task<bool> CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        DbSet.Add(entity);
        return Task.FromResult(true);
    }

    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        int affected = await DbSet.Where(e => e.Id == id).ExecuteDeleteAsync(ct);

        return affected > 0;
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllAsync<T>(
        Expression<Func<TEntity, T>> selector,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    )
    {
        return await DbSet.Select(selector).Skip(skip).Take(take).ToListAsync(ct);
    }

    public virtual async Task<IReadOnlyCollection<T>> GetAllByIdsAsync<T>(
        IEnumerable<Guid> ids,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    )
    {
        return await DbSet.Where(e => ids.Contains(e.Id)).Select(selector).ToListAsync(ct);
    }

    public virtual async Task<T?> GetByIdAsync<T>(
        Guid id,
        Expression<Func<TEntity, T>> selector,
        CancellationToken ct = default
    )
    {
        return await DbSet.Where(e => e.Id == id).Select(selector).FirstOrDefaultAsync(ct);
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        return DbContext.SaveChangesAsync(ct);
    }
}
