namespace ites.Application.Interfaces.Services
{
    public interface IApplicationsService
    {
        public Task CreateAsync(Guid from, Guid to);
        public Task<Core.Models.Application> GetAsync(Guid id);
        public Task<IList<Core.Models.Application>> GetAsync(ICollection<Guid> ids);
        public Task DeleteAsync(Guid id);
    }
}
