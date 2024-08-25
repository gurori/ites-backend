namespace ites.Application.Interfaces.Repositories
{
    public interface IApplicationsRepository
    {
        public Task<IList<Core.Models.Application>> GetAsync(IList<Guid> ids);
        public Task CreateForCompetitionAsync(Core.Models.Application application);
        public Task HandleCompetitionAsync(Guid id, bool isAccept);
        public Task CreateForOrderAsync(Core.Models.Application application);
        public Task HandleOrderAsync(Guid id, bool isAccept);
        public Task CreateForTeamAsync(Core.Models.Application application);
        public Task HandleTeamAsync(Guid id, bool isAccept);
    }
}
