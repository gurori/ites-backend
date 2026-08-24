using ites.Application.Constants;
using ites.Application.Contracts.Moderation;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class ModerationService(
    ITeamRepository teamRepository,
    IOrdersRepository orderRepository
) : IModerationService
{
    public async Task<ModerationResponse> GetAllAsync(CancellationToken ct = default)
    {
        var ordersTask = orderRepository.GetByVisibilityAsync(
            o => new OrderResponse(o.Id, o.Title, o.Description, o.Price, o.DeadLine),
            false,
            ct: ct
        );

        var teamsTask = teamRepository.GetByVisibilityAsync(
            t => new TeamResponse(
                t.Id,
                t.Name,
                t.Description,
                t.Members.Select(m => new MemberSummaryResponse(
                        m.Id,
                        m.LastName,
                        m.FirstName,
                        m.MiddleName,
                        m.Description,
                        m.JobTitle
                    ))
                    .ToArray(),
                t.AdminId
            ),
            false,
            ct: ct
        );

        await Task.WhenAll(ordersTask, teamsTask);

        return new ModerationResponse(await teamsTask, await ordersTask);
    }

    public async Task HandleAsync(string type, Guid id, bool accept, CancellationToken ct = default)
    {
        switch (type.Trim().ToLower())
        {
            case ModerationTarget.Team:
                if (accept)
                    await teamRepository.SetIsPublicAsync(id, true, ct);
                else
                    await teamRepository.DeleteAsync(id, ct);
                break;
            case ModerationTarget.Order:
                if (accept)
                    await orderRepository.SetIsPublicAsync(id, true, ct);
                else
                    await orderRepository.DeleteAsync(id, ct);
                break;
            default:
                throw new ArgumentException($"Invalid type: {type}", nameof(type));
        }
    }
}
