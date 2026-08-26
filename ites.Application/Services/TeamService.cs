using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Enums;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
    : ITeamService
{
    public async Task<Guid> AddJoinRequestAsync(
        Guid userId,
        Guid teamId,
        AddTeamJoinRequestDto request,
        CancellationToken ct = default
    )
    {
        var team =
            await teamRepository.GetByIdAsync(
                teamId,
                t => new { t.IsPublic, MembersCount = t.Members.Count },
                ct
            ) ?? throw new NotFoundException("Команда не найдена.");

        if (!team.IsPublic)
            throw new BadRequestException("Команда закрыта для вступления.");

        if (team.MembersCount >= 5)
            throw new BadRequestException("В команде уже максимальное количество участников.");

        var joinRequest = new TeamJoinRequest
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            TeamId = teamId,
            CoverLetter = request.CoverLetter ?? string.Empty,
            Status = RequsetStatus.Pending,
        };

        await teamRepository.AddTeamJoinRequestAsync(joinRequest, ct);
        await teamRepository.SaveChangesAsync(ct);
        return joinRequest.Id;
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

    public async Task HandleJoinRequestAsync(
        Guid userId,
        Guid joinRequestId,
        HandleTeamJoinRequestDto request,
        CancellationToken ct = default
    )
    {
        var joinRequest =
            await teamRepository.GetJoinRequestByIdAsync(joinRequestId, ct)
            ?? throw new NotFoundException("Заявка не найдена.");

        if (joinRequest.Status != RequsetStatus.Pending)
            throw new BadRequestException("Эта заявка уже обработана.");

        var adminId = await teamRepository.GetByIdAsync(joinRequest.TeamId, t => t.AdminId, ct);

        if (adminId != userId)
            throw new ForbiddenException("У вас нет прав для обработки заявок этой команды.");

        if (request.Accept)
        {
            var team =
                await teamRepository.GetByIdAsync(joinRequest.TeamId, ct)
                ?? throw new NotFoundException("Команда не найдена.");

            if (team.Members.Count >= 5)
                throw new BadRequestException("Команда уже заполнена.");

            joinRequest.Status = RequsetStatus.Accepted;

            await teamRepository.AddMemberToTeamAsync(team.Id, joinRequest.UserId, ct);

            if (team.Members.Count + 1 >= 5)
            {
                team.IsPublic = false;
                await teamRepository.UpdateAsync(team, ct);
            }
        }
        else
        {
            joinRequest.Status = RequsetStatus.Rejected;
        }

        await teamRepository.UpdateJoinRequestAsync(joinRequest, ct);
        await teamRepository.SaveChangesAsync(ct);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
