using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IRequestEntityRepository
{
    public Task<IList<RequestEntity>> GetAsync(ICollection<Guid> ids);
    public Task CreateForCompetitionAsync(RequestEntity application);
    public Task HandleCompetitionAsync(Guid id, bool isAccept);
    public Task CreateForOrderAsync(RequestEntity application);
    public Task HandleOrderAsync(Guid id, bool isAccept);
    public Task CreateForTeamAsync(RequestEntity application);
    public Task HandleTeamAsync(Guid id, bool isAccept);
}
