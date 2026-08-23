using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public class UserRepository(ItesDbContext context)
        : CrudRepository<User>(context),
            IUserRepository
    {
        public Task<T?> GetByEmailAsync<T>(
            string email,
            Expression<Func<User, T>> selector,
            CancellationToken ct = default
        )
        {
            return DbSet.Where(u => u.Email == email).Select(selector).FirstOrDefaultAsync(ct);
        }
    }
}
