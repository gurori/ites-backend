using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IOrdersRepository : IRepository<Order>
{
    public Task<IList<Order>> GetAllPublicAsync();
    public Task<IList<Order>> GetAllNotPublicAsync();
    public Task SetIsPublicAsync(Guid id, bool isPublic);
}
