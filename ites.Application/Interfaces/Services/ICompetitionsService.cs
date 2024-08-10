using ites.Core.Models;

namespace ites.Application.Interfaces.Services
{
    public interface ICompetitionsService
    {
        public Task CreateAsync(string token, string title, string description, DateTime startDate, DateTime endDate);
        public Task<Competition> GetAsync(Guid id);
        public Task<IList<Competition>> GetAsync();
        public Task<IList<Competition>> GetAsync(IList<Guid> ids);
        public Task AddApplicationAsync(string token, Guid forId);
        public Task HandleApplicationAsync(Guid id, bool isAccept);
        public Task UpdateAsync(Guid id, string title, string description, DateTime startDate, DateTime endDate);
        public Task DeleteAsync(Guid id);

    }
}
