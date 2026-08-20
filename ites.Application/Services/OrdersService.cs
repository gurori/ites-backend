using ites.Application.Interfaces.Services;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;
using ites.Core.Models;

namespace ites.Application.Services;

public sealed class OrdersService(
    IOrdersRepository ordersRepository,
    IApplicationsRepository applicationsRepository
) : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository = ordersRepository;
    private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;

    public async Task AddApplicationAsync(Guid userId, Guid forId)
    {
        Core.Models.Application application = new(Guid.Empty, userId, forId);
        await _applicationsRepository.CreateForOrderAsync(application);
    }

    public async Task CreateAsync(
        Guid userId,
        string title,
        string description,
        decimal price,
        DateTime deadLine
    )
    {
        Order order = new(Guid.Empty, title, description, price, deadLine);
        await _ordersRepository.CreateAsync(userId, order);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetAsync(Guid id)
    {
        Order order =
            await _ordersRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Заказ не найден");
        return order;
    }

    public async Task<IList<Order>> GetAsync()
    {
        return await _ordersRepository.GetAllPublicAsync();
    }

    public async Task<IList<Order>> GetAsync(ICollection<Guid> ids)
    {
        return await _ordersRepository.GetWithIdsAsync(ids);
    }

    public async Task HandleApplicationAsync(Guid id, bool isAccept)
    {
        await _applicationsRepository.HandleOrderAsync(id, isAccept);
    }

    public Task UpdateAsync(
        Guid userId,
        Guid id,
        string title,
        string description,
        decimal price,
        DateTime deadLine
    )
    {
        throw new NotImplementedException();
    }
}
