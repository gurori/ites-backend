using ites.Application.Interfaces.Services;
using ites.Core.Entities;
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

        public Task<RequestEntity> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<RequestEntity>> GetAsync(ICollection<Guid> ids)
        {
            return await _applicationsRepository.GetAsync(ids);
        }
    }
}
