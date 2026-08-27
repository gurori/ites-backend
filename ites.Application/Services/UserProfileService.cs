using ites.Application.Contracts;
using ites.Application.Contracts.Applications;
using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class UserProfileService(
    ICompetitionsService competitionsService,
    IUserService userService,
    IUserRepository userRepository,
    IOrdersService ordersService,
    ITeamService teamService
) : IUserProfileService
{
    public async Task<MemberResponse> GetMemberAsync(Guid id, CancellationToken ct = default)
    {
        var member = await userRepository.GetByIdAsync(
            id,
            m => new MemberResponse(
                m.Id,
                m.LastName,
                m.FirstName,
                m.MiddleName,
                m.Email,
                m.Role,
                m.Description,
                m.JobTitle,
                m.TeamId,
                m.Competitions.Select(c => new CompetitionSummaryResponse(c.Id, c.ContentInHtml)),
                [],
                m.Orders
            ),
            ct
        );
        User user =
            await userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Пользователь не найден");

        IList<Competition> competitions = await competitionsService.GetAsync(user.CompetitionsIds);
        IList<Competition> competitionsApplications = await competitionsService.GetAsync(
            user.ApplicationsForCompetitions
        );

        IList<Order> orders = await ordersService.GetAsync(user.OrdersIds);
        IList<Order> ordersApplications = await ordersService.GetAsync(user.ApplicationsForOrders);

        IList<Team> teamsApplications = await teamService.GetAsync(user.ApplicationsForTeams);
        var applicationsIds = await _applicationsService.GetAsync(user.ApplicationsIds);
        IList<TeamJoinRequestDto> applications = [];

        foreach (var application in applicationsIds)
        {
            UserProfileResponse fromMember = await userService.GetAsync(application.From);
            applications.Add(new(application.Id, fromMember));
        }

        MemberResponse member = new(
            user.Id,
            user.LastName,
            user.FirstName,
            user.MiddleName,
            user.Email,
            user.Role,
            user.Description,
            user.JobTitle,
            competitions,
            competitionsApplications,
            orders,
            ordersApplications,
            teamsApplications,
            user.TeamId,
            applications
        );
        return member;
    }

    public async Task<OrganizerResponse> GetOrganizerAsync(Guid id, CancellationToken ct = default)
    {
        User user =
            await userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Пользователь не найден");

        IList<Competition> competitions = await competitionsService.GetAsync(user.CompetitionsIds);
        IList<RequestEntity> applicationsIds = await _applicationsService.GetAsync(
            user.ApplicationsIds
        );
        IList<CompetitionEntryResponse> applications = [];

        foreach (RequestEntity a in applicationsIds)
        {
            UserProfileResponse fromMember = await userService.GetAsync(a.From);
            Competition competition = await competitionsService.GetAsync(a.For);
            CompetitionResponse forCompetition = _mapper.Map<CompetitionResponse>(competition);

            applications.Add(new(a.Id, fromMember, forCompetition));
        }

        OrganizerResponse organizer = new(
            user.Id,
            user.LastName,
            user.FirstName,
            user.MiddleName,
            user.Email,
            user.Role,
            user.Description,
            user.JobTitle,
            competitions,
            applications
        );
        return organizer;
    }

    public async Task<ClientResponse> GetClientAsync(Guid id, CancellationToken ct = default)
    {
        User user =
            await userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Пользователь не найден");

        IList<Order> orders = await ordersService.GetAsync(user.OrdersIds);
        IList<RequestEntity> applicationsIds = await _applicationsService.GetAsync(
            user.ApplicationsIds
        );
        IList<OrderBidResponse> applications = [];

        foreach (RequestEntity a in applicationsIds)
        {
            UserProfileResponse fromMember = await userService.GetAsync(a.From);
            Order order = await ordersService.GetAsync(a.For);
            OrderResponse forOrder = _mapper.Map<OrderResponse>(order);

            applications.Add(new(a.Id, fromMember, forOrder));
        }

        ClientResponse client = new(
            user.Id,
            user.LastName,
            user.FirstName,
            user.MiddleName,
            user.Email,
            user.Role,
            user.Description,
            user.JobTitle,
            orders,
            applications
        );
        return client;
    }
}
