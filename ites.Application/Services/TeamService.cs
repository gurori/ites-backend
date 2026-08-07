using AutoMapper;
using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;
using ites.Core.Models;

namespace ites.Application.Services;

public sealed class TeamService(
    ITeamRepository teamRepo,
    IApplicationsRepository applicationsRepo,
    IUserRepository userRepo,
    IMapper mapper
) : ITeamService
{
    private readonly ITeamRepository _teamRepo = teamRepo;
    private readonly IApplicationsRepository _applicationsRepo = applicationsRepo;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IMapper _mapper = mapper;

    public async Task AddApplicationAsync(Guid userId, Guid forId)
    {
        Core.Models.Application application = new(Guid.Empty, userId, forId);
        await _applicationsRepo.CreateForTeamAsync(application);
    }

    public async Task CreateAsync(Guid userId, string name, string description)
    {
        Team team = new(name, description, userId);
        await _teamRepo.CreateAsync(team);
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<TeamResponse> GetAsync(Guid id)
    {
        Team team =
            await _teamRepo.GetByIdAsync(id) ?? throw new NotFoundException("Команда не найдена");

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
}
