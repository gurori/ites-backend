namespace ites.Application.Interfaces.Repositories
{
    public interface IApplicationsRepository
    {
        public Task<IList<Core.Models.Application>> GetManyByIdsAsync(IList<Guid> ids);
        public Task CreateForCompetitionAsync(Core.Models.Application application);
        public Task HandleCompetitionAsync(Guid id, bool isAccept);
    }
}
