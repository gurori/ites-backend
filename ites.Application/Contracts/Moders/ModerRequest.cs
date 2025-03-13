using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;

namespace ites.Application.Contracts.Moders
{
    public record ModerResponse(TeamResponse[] Teams, OrderResponse[] Orders);
}
