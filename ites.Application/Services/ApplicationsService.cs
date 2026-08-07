using ites.Application.Interfaces.Services;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services
{
    public sealed class ApplicationsService(IApplicationsRepository applicationsRepository)
                : IApplicationsService
    {
        private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;

        public Task CreateAsync(Guid from, Guid to)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Application> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Core.Models.Application>> GetAsync(IList<Guid> ids)
        {
            return await _applicationsRepository
                .GetAsync(ids);
        }
    }
}
