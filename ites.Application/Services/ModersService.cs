using AutoMapper;
using ites.Application.Contracts.Moders;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;

namespace ites.Application.Services
{
    public sealed class ModersService(
        ITeamRepository teamRepository,
        IOrdersRepository orderRepository,
        IMapper mapper
    ) : IModersService
    {
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IOrdersRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ModerResponse> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllNotPublicAsync();
            var teams = await _teamRepository.GetAllNotPublicAsync();

            return new ModerResponse(
                _mapper.Map<TeamResponse[]>(teams),
                _mapper.Map<OrderResponse[]>(orders)
            );
        }

        public async Task HandleAsync(string type, Guid id, bool accept)
        {
            if (type == "team")
            {
                if (accept)
                {
                    await _teamRepository.SetIsPublicAsync(id, true);
                }
                else
                {
                    await _teamRepository.DeleteAsync(id);
                }
            }
            else
            {
                if (accept)
                {
                    await _orderRepository.SetIsPublicAsync(id, true);
                }
                else
                {
                    await _orderRepository.DeleteAsync(id);
                }
            }
        }
    }
}
