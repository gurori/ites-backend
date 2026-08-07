using ites.Core.Models;

namespace ites.Core.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<bool> CreateAsync(User user);

    public Task<User?> GetByEmailAsync(string email);

    public Task<User?> GetByIdAsync(Guid id);
    public Task UpdateAsync(
        Guid id,
        string lastName,
        string firstName,
        string middleName,
        string description,
        string jobTitle
    );
    public Task<string?> GetRoleByIdAsync(Guid id);
    public Task<IList<User>> GetManyByIdAsync(IList<Guid> ids);
    public Task DeleteByIdAsync(Guid id);
}
