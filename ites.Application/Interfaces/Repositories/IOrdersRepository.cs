using ites.Core.Models;

namespace ites.Application.Interfaces.Repositories
{
    public interface IOrdersRepository
    {
        public Task CreateAsync(Guid clientId, Order order);
        public Task<Order?> GetByIdAsync(Guid id);
        public Task<IList<Order>> GetAllAsync();
        public Task<IList<Order>> GetWithIdsAsync(IList<Guid> ids);
        public Task DeleteAsync(Guid id);
    }
}
