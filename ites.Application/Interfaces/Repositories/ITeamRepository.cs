using ites.Core.Models;

namespace ites.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        public Task CreateAsync(Team team);
        public Task<Team?> GetByIdAsync(Guid id);
        public Task<IList<Team>> GetAllAsync();
        public Task<IList<Team>> GetByIdsAsync(IList<Guid> ids);
        public Task DeleteAsync(Guid id);
    }
}
