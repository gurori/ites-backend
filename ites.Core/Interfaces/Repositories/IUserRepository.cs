using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<TResult?> GetByEmailAsync<TResult>(
        string email,
        Expression<Func<User, TResult>> selector,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );
}
