using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public abstract class BaseRepository<TEntity>(ItesDbContext dbContext) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ItesDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public virtual Task CreateAsync(TEntity entity, CancellationToken ct = default)
    {
        DbSet.Add(entity);
        return Task.CompletedTask;
    }

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return DbSet.CountAsync(ct);
    }

    public Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default
    )
    {
        return DbSet.CountAsync(predicate, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        return DbContext.SaveChangesAsync(ct);
    }

    public Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default
    )
    {
        return DbSet.AnyAsync(predicate, ct);
    }
}
