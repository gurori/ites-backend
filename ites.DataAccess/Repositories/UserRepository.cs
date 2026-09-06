using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class UserRepository(ItesDbContext context)
    : CrudRepository<User>(context),
        IUserRepository
{
    public Task<TResult?> GetByEmailAsync<TResult>(
        string email,
        Expression<Func<User, TResult>> selector,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return BuildQuery<User>(null, asSplitQuery)
            .Where(u => u.Email == email)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    }
}
