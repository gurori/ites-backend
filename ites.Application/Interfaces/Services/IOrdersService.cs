using ites.Core.Entities;

namespace ites.Application.Interfaces.Services
{
    public interface IOrdersService
    {
        public Task CreateAsync(
            Guid userId,
            string title,
            string description,
            decimal price,
            DateTime deadLine
        );
        public Task<Order> GetAsync(Guid id);
        public Task<IList<Order>> GetAsync();
        public Task<IList<Order>> GetAsync(ICollection<Guid> ids);
        public Task AddApplicationAsync(Guid userId, Guid forId);
        public Task HandleApplicationAsync(Guid id, bool isAccept);
        public Task UpdateAsync(
            Guid userId,
            Guid id,
            string title,
            string description,
            decimal price,
            DateTime deadLine
        );
        public Task DeleteAsync(Guid id);
    }
}
