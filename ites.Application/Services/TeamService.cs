using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class TeamService(
    ITeamRepository teamRepository,
    IRequestEntityRepository requestEntityRepository,
    IUserRepository userRepository
) : ITeamService
{
    public async Task AddApplicationAsync(Guid userId, Guid teamId, CancellationToken ct = default)
    {
        RequestEntity application = new()
        {
            Id = Guid.CreateVersion7(),
            For = teamId,
            From = userId,
        };

        await requestEntityRepository.CreateForTeamAsync(application, ct);
        await requestEntityRepository.SaveChangesAsync(ct);
    }

    public async Task<Guid> CreateAsync(
        Guid userId,
        TeamRequest request,
        CancellationToken ct = default
    )
    {
        var admin =
            await userRepository.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("Пользователь не найден");

        Team team = new()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            AdminId = userId,
            Members = [admin],
        };

        await teamRepository.CreateAsync(team, ct);
        await teamRepository.SaveChangesAsync(ct);
        return team.Id;
    }

    public async Task<TeamResponse> GetAsync(Guid id, CancellationToken ct = default)
    {
        var team =
            await teamRepository.GetByIdAsync(
                id,
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
                ct
            ) ?? throw new NotFoundException("Команда не найдена");

        return team;
    }

    public async Task<TeamListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var teams = await teamRepository.GetByVisibilityAsync(
            t => new TeamSummaryResponse(t.Id, t.Name, t.Description, t.Members.Count),
            isPublic: true,
            skip: (page - 1) * pageSize,
            take: pageSize,
            ct
        );

        var totalCount = await teamRepository.CountAsync(t => t.IsPublic, ct);

        return new TeamListResponse(teams, totalCount, page, pageSize);
    }

    public async Task HandleApplicationAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        await requestEntityRepository.HandleTeamAsync(id, isAccept, ct);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
