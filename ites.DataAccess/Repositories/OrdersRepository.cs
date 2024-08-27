using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.Core.Models;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class OrdersRepository(
        ItesDbContext context,
        IMapper mapper)
            : IOrdersRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task CreateAsync(Guid clientId, Order order)
        {
            UserEntity? client = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == clientId);

            if (client is null) return;

            OrderEntity orderEntity = new()
            {
                Id = Guid.NewGuid(),
                Title = order.Title,
                Description = order.Description,
                Price = order.Price,
                DeadLine = order.DeadLine,
                ClientId = clientId,
                IsPublic = true,
            };

            await _context.Orders.AddAsync(orderEntity);
            client.OrdersIds.Add(orderEntity.Id);
            _context.Users.Update(client);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Order>> GetAllPublicAsync()
        {
            IList<OrderEntity> orders = await _context.Orders
                .AsNoTracking()
                .Where(o => o.IsPublic)
                .ToListAsync();

            return _mapper.Map<Order[]>(orders);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            OrderEntity? order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order is null) return null;

            return _mapper.Map<Order>(order);
        }

        public async Task<IList<Order>> GetWithIdsAsync(IList<Guid> ids)
        {
            IList<OrderEntity> orders = await _context.Orders
                .AsNoTracking()
                .Where(o => ids.Contains(o.Id))
                .ToListAsync();

            return _mapper.Map<Order[]>(orders);
        }
    }
}
