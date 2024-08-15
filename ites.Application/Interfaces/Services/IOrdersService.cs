using ites.Core.Models;

namespace ites.Application.Interfaces.Services
{
    public interface IOrdersService
    {
        public Task CreateAsync(
            string token,
            string title,
            string description,
            decimal price,
            DateTime deadLine);
        public Task<Order> GetAsync(Guid id);
        public Task<IList<Order>> GetAsync();
        public Task<IList<Order>> GetAsync(IList<Guid> ids);
        public Task AddApplicationAsync(string token, Guid forId);
        public Task HandleApplicationAsync(Guid id, bool isAccept);
        public Task UpdateAsync(
            string token,
            Guid id,
            string title,
            string description,
            decimal price,
            DateTime deadLine);
        public Task DeleteAsync(Guid id);
    }
}
