using AutoMapper;
using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Exeptions;
using ites.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace ites.Application.Services
{
    public sealed class TeamService(
        ITeamRepository teamRepo,
        IJwtProvider jwtProvider,
        IApplicationsRepository applicationsRepo,
        IUserRepository userRepo,
        IMapper mapper
    ) : ITeamService
    {
        private readonly ITeamRepository _teamRepo = teamRepo;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IApplicationsRepository _applicationsRepo = applicationsRepo;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IMapper _mapper = mapper;

        public async Task AddApplicationAsync(string token, Guid forId)
        {
            Guid fromId = await GetUserIdFromTokenAsync(token);
            Core.Models.Application application = new(Guid.Empty, fromId, forId);
            await _applicationsRepo.CreateForTeamAsync(application);
        }

        public async Task CreateAsync(string token, string name, string description)
        {
            Guid adminId = await GetUserIdFromTokenAsync(token);
            Team team = new(name, description, adminId);
            await _teamRepo.CreateAsync(team);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TeamResponse> GetAsync(Guid id)
        {
            Team team =
                await _teamRepo.GetByIdAsync(id)
                ?? throw new NotFoundException("Команда не найдена");

            IList<User> members = await _userRepo.GetManyByIdAsync(team.MembersIds);

            return new(
                team.Id,
                team.Name,
                team.Description,
                _mapper.Map<UserProfileResponse[]>(members),
                team.AdminId
            );
        }

        public async Task<IList<Team>> GetAsync()
        {
            return await _teamRepo.GetAllPublicAsync();
        }

        public async Task<IList<Team>> GetAsync(IList<Guid> ids)
        {
            return await _teamRepo.GetByIdsAsync(ids);
        }

        public async Task HandleApplicationAsync(Guid id, bool isAccept)
        {
            await _applicationsRepo.HandleTeamAsync(id, isAccept);
        }

        private async Task<Guid> GetUserIdFromTokenAsync(string token)
        {
            TokenValidationResult validationResult = await _jwtProvider.ValidateTokenAsync(token);

            if (!validationResult.IsValid)
                throw new UnauthorizedException();

            string id =
                validationResult.Claims[CustomClaims.UserId].ToString()
                ?? throw new UnauthorizedException();

            return Guid.Parse(id);
        }
    }
}
