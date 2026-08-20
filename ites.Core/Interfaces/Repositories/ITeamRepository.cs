using ites.Core.Models;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository
{
    public Task CreateAsync(Team team);
    public Task<Team?> GetByIdAsync(Guid id);
    public Task<IList<Team>> GetAllPublicAsync();
    public Task<IList<Team>> GetAllNotPublicAsync();
    public Task<IList<Team>> GetByIdsAsync(ICollection<Guid> ids);
    public Task DeleteAsync(Guid id);
    public Task SetIsPublicAsync(Guid id, bool isPublic);
}
