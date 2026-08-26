using System.Linq.Expressions;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class TeamRepository(ItesDbContext context)
    : CrudRepository<Team>(context),
        ITeamRepository
{
    public Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default)
    {
        return DbSet
            .Where(t => t.Id == id)
            .ExecuteUpdateAsync(t => t.SetProperty(t => t.IsPublic, isPublic), ct);
    }

    public async Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Team, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    )
    {
        return await DbSet
            .Where(t => t.IsPublic == isPublic)
            .Skip(skip)
            .Take(take)
            .Select(selector)
            .ToListAsync(ct);
    }

    public Task AddTeamJoinRequestAsync(
        TeamJoinRequest teamJoinRequest,
        CancellationToken ct = default
    )
    {
        DbContext.TeamJoinRequests.Add(teamJoinRequest);
        return Task.CompletedTask;
    }

    public async Task<Team?> GetWithMembersByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await DbContext
            .Teams.Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task AddMemberToTeamAsync(Guid teamId, Guid userId, CancellationToken ct = default)
    {
        await DbContext
            .Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.TeamId, teamId), ct);
    }

    public async Task<TeamJoinRequest?> GetJoinRequestByIdAsync(
        Guid id,
        CancellationToken ct = default
    )
    {
        return await DbContext.TeamJoinRequests.FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public Task UpdateJoinRequestAsync(TeamJoinRequest joinRequest, CancellationToken ct = default)
    {
        DbContext.TeamJoinRequests.Update(joinRequest);
        return Task.CompletedTask;
    }
}
