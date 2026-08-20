using ites.Core.Models;

namespace ites.Core.Interfaces.Repositories;

public interface ICompetitionsRepository
{
    public Task<bool> CreateAsync(Guid userId, Competition competition);
    public Task<Competition?> GetByIdAsync(Guid id);
    public Task<IList<Competition>> GetAllAsync();
    public Task<IList<Competition>> GetAllWithIdAsync(ICollection<Guid> ids);
    public Task UpdateAsync(
        Guid id,
        string title,
        string description,
        DateTime startDate,
        DateTime endDate
    );
    public Task DeleteAsync(Guid id);
}
