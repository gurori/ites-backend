using AutoMapper;
using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;
using ites.Application.Interfaces.Services;
using ites.Core.Enums;
using ites.Core.Models;
using ites.Core.Problems;
using Microsoft.IdentityModel.Tokens;

namespace ites.Application.Services
{
    public sealed class CompetitionsService(
        IMapper mapper,
        ICompetitionsRepository competitionsRepository,
        IJwtProvider jwtProvider,
        IUserService userService,
        IApplicationsRepository applicationsRepository)
            : ICompetitionsService
    {
        private readonly ICompetitionsRepository _competitionsRepository = competitionsRepository;
        private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task AddApplicationAsync(string token, Guid forId)
        {
            Guid fromMemberId = await _userService
                .GetIdFromTokenAsync(token);
            Core.Models.Application application = new(Guid.Empty, fromMemberId, forId);
            await _applicationsRepository
                .CreateForCompetitionAsync(application);

        }

        public async Task CreateAsync(string token, string title, string description, DateTime startDate, DateTime endDate)
        {
            Guid orgId = await GetUserIdFromTokenAsync(token);
            Competition competition = new(Guid.NewGuid(), title, description, startDate, endDate);

            bool isCreated = await _competitionsRepository
                .CreateAsync(orgId, competition);

            if (!isCreated) throw UserProblem.NotFound;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Competition> GetAsync(Guid id)
        {
            Competition competition = await _competitionsRepository
                .GetByIdAsync(id)
                    ?? throw CompetitionProblem.NotFound;

            return competition;
        }

        public async Task<IList<Competition>> GetAsync()
        {
            return await _competitionsRepository
                .GetAllAsync();
        }

        public async Task<IList<Competition>> GetAsync(IList<Guid> ids)
        {
            return await _competitionsRepository
                .GetAllWithIdAsync(ids);
        }

        public async Task HandleApplicationAsync(Guid id, bool isAccept)
        {
            await _applicationsRepository
                .HandleCompetitionAsync(id, isAccept);
        }

        public Task UpdateAsync(Guid id, string title, string description, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> GetUserIdFromTokenAsync(string token)
        {
            TokenValidationResult validationResult = await _jwtProvider
                .ValidateTokenAsync(token);

            if (!validationResult.IsValid)
                throw UserProblem.TokenProblem;

            string id = validationResult.Claims[CustomClaims.UserId].ToString()
                ?? throw UserProblem.TokenProblem;

            return Guid.Parse(id);
        }
    }
}
