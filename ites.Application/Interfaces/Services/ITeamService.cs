using ites.Application.Contracts.Teams;
using ites.Core.Models;

namespace ites.Application.Interfaces.Services
{
    public interface ITeamService
    {
        public Task CreateAsync(Guid userId, string name, string description);
        public Task<TeamResponse> GetAsync(Guid id);
        public Task<IList<Team>> GetAsync();
        public Task<IList<Team>> GetAsync(IList<Guid> ids);
        public Task AddApplicationAsync(Guid userId, Guid forId);
        public Task HandleApplicationAsync(Guid id, bool isAccept);
        public Task DeleteAsync(Guid id);
    }
}
