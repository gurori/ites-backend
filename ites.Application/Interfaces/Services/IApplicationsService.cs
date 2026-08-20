using ites.Core.Entities;

namespace ites.Application.Interfaces.Services
{
    public interface IRequestEntityService
    {
        public Task CreateAsync(Guid from, Guid to);
        public Task<RequestEntity> GetAsync(Guid id);
        public Task<IList<RequestEntity>> GetAsync(ICollection<Guid> ids);
        public Task DeleteAsync(Guid id);
    }
}
