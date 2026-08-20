namespace ites.Core.Interfaces.Repositories;

public interface IApplicationsRepository
{
    public Task<IList<Models.Application>> GetAsync(ICollection<Guid> ids);
    public Task CreateForCompetitionAsync(Models.Application application);
    public Task HandleCompetitionAsync(Guid id, bool isAccept);
    public Task CreateForOrderAsync(Models.Application application);
    public Task HandleOrderAsync(Guid id, bool isAccept);
    public Task CreateForTeamAsync(Models.Application application);
    public Task HandleTeamAsync(Guid id, bool isAccept);
}
