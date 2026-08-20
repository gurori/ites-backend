using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);
}
