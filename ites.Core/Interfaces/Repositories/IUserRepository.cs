using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    public Task<T?> GetByEmailAsync<T>(
        string email,
        Expression<Func<User, T>> selector,
        CancellationToken ct = default
    );
}
