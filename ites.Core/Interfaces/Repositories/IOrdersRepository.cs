using ites.Core.Models;

namespace ites.Core.Interfaces.Repositories;

public interface IOrdersRepository
{
    public Task CreateAsync(Guid clientId, Order order);
    public Task<Order?> GetByIdAsync(Guid id);
    public Task<IList<Order>> GetAllPublicAsync();
    public Task<IList<Order>> GetAllNotPublicAsync();
    public Task<IList<Order>> GetWithIdsAsync(ICollection<Guid> ids);
    public Task DeleteAsync(Guid id);
    public Task SetIsPublicAsync(Guid id, bool isPublic);
}
