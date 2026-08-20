using ites.Core.Entities;

namespace ites.Application.Interfaces.Services
{
    public interface ICompetitionsService
    {
        public Task CreateAsync(Guid userId, string contentInHtml);
        public Task<Competition> GetAsync(Guid id);
        public Task<IList<Competition>> GetAsync();
        public Task<IList<Competition>> GetAsync(ICollection<Guid> ids);
        public Task AddApplicationAsync(Guid userId, Guid forId);
        public Task HandleApplicationAsync(Guid id, bool isAccept);
        public Task UpdateAsync(
            Guid id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate
        );
        public Task DeleteAsync(Guid id);
    }
}
