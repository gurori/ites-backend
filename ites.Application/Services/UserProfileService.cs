using AutoMapper;
using ites.Application.Contracts;
using ites.Application.Contracts.Competitions;
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
        IMapper mapper)
                : IUserProfileService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;
        private readonly ICompetitionsService _competitionsService = competitionsService;
        private readonly IApplicationsService _applicationsService = applicationsService;
        private readonly IMapper _mapper = mapper;

        public async Task<MemberResponse> GetMemberAsync(string token)
        {
            await Console.Out.WriteLineAsync("TOKEN - " + token);
            Guid id = await _userService.GetIdFromTokenAsync(token);
            await Console.Out.WriteLineAsync("ID - " + id.ToString());
            return await GetMemberByIdAsync(id);
        }

        public async Task<MemberResponse> GetMemberAsync(Guid id)
        {
             return await GetMemberByIdAsync(id);
        }

        public async Task<OrganizerResponse> GetOrganizerAsync(string token)
        {
            Guid id = await _userService.GetIdFromTokenAsync(token);
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

        private async Task<MemberResponse> GetMemberByIdAsync(Guid id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            await Console.Out.WriteLineAsync("USER - " + user.FirstName);

            IList<Competition> competitions = await _competitionsService
                .GetAsync(user.CompetitionsIds);
            Console.WriteLine("comp = " + competitions.Count);
            IList<Competition> competitionsApplications = await _competitionsService
                .GetAsync(user.ApplicationsForCompetitions);
            Console.WriteLine("comp apple = " + competitionsApplications.Count);

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
                competitionsApplications
                );
            return member;
        }
    }
}