using AutoMapper;
using ites.Application.Contracts;
using ites.Application.Contracts.Applications;
using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Core.Models;

namespace ites.Application.Services
{
    public sealed class UserProfileService(
        ICompetitionsService competitionsService,
        IApplicationsService applicationsService,
        IUserService userService,
        IUserRepository userRepository,
        IOrdersService ordersService,
        ITeamService teamService,
        IMapper mapper)
                : IUserProfileService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly ICompetitionsService _competitionsService = competitionsService;
        private readonly IOrdersService _ordersService = ordersService;
        private readonly IApplicationsService _applicationsService = applicationsService;
        private readonly ITeamService _teamService = teamService;
        private readonly IMapper _mapper = mapper;

        public async Task<ClientResponse> GetClientAsync(string token)
        {
            Guid id = await _userService.GetIdFromTokenAsync(token);
            return await GetClientByIdAsync(id);
        }

        public async Task<ClientResponse> GetClientAsync(Guid id)
        {
            return await GetClientByIdAsync(id);
        }

        public async Task<MemberResponse> GetMemberAsync(string token)
        {
            Guid id = await _userService.GetIdFromTokenAsync(token);
            return await GetMemberByIdAsync(id);
        }

        public async Task<MemberResponse> GetMemberAsync(Guid id)
        {
             return await GetMemberByIdAsync(id);
        }

        public async Task<OrganizerResponse> GetOrganizerAsync(string token)
        {
            Guid id = await _userService.GetIdFromTokenAsync(token);
            return await GetOrganizerByIdAsync(id);
        }

        public async Task<OrganizerResponse> GetOrganizerAsync(Guid id)
        {
            return await GetOrganizerByIdAsync(id);
        }

        private async Task<MemberResponse> GetMemberByIdAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);

            IList<Competition> competitions = await _competitionsService
                .GetAsync(user.CompetitionsIds);
            IList<Competition> competitionsApplications = await _competitionsService
                .GetAsync(user.ApplicationsForCompetitions);

            IList<Order> orders = await _ordersService
                .GetAsync(user.OrdersIds);
            IList<Order> ordersApplications = await _ordersService
                .GetAsync(user.ApplicationsForOrders);

            IList<Team> teamsApplications = await _teamService
                .GetAsync(user.ApplicationsForTeams);

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
                user.TeamId
                );
            return member;
        }
        private async Task<OrganizerResponse> GetOrganizerByIdAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);

            IList<Competition> competitions = await _competitionsService
                .GetAsync(user.CompetitionsIds);
            IList<Core.Models.Application> applicationsIds = await _applicationsService
                .GetAsync(user.ApplicationsIds);
            IList<CompetitionApplicationResponse> applications = [];

            foreach (Core.Models.Application a in applicationsIds)
            {
                UserProfileResponse fromMember = await _userService
                    .GetAsync(a.From);
                Competition competition = await _competitionsService
                    .GetAsync(a.For);
                CompetitionResponse forCompetition = _mapper
                    .Map<CompetitionResponse>(competition);

                applications.Add(new(
                    a.Id,
                    fromMember,
                    forCompetition
                    ));
            };

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

        private async Task<ClientResponse> GetClientByIdAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);

            IList<Order> orders = await _ordersService
                .GetAsync(user.OrdersIds);
            IList<Core.Models.Application> applicationsIds = await _applicationsService
                .GetAsync(user.ApplicationsIds);
            IList<OrderApplicationResponse> applications = [];

            foreach (Core.Models.Application a in applicationsIds)
            {
                UserProfileResponse fromMember = await _userService
                    .GetAsync(a.From);
                Order order = await _ordersService
                    .GetAsync(a.For);
                OrderResponse forOrder = _mapper
                    .Map<OrderResponse>(order);

                applications.Add(new(
                    a.Id,
                    fromMember,
                    forOrder
                    ));
            };

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
}