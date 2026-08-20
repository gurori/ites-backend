using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository : IRepository<Team>
{
    public Task<IList<Team>> GetAllPublicAsync();
    public Task<IList<Team>> GetAllNotPublicAsync();
    public Task SetIsPublicAsync(Guid id, bool isPublic);
}
