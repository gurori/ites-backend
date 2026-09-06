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

    public Task AddTeamJoinRequestAsync(
        TeamJoinRequest teamJoinRequest,
        CancellationToken ct = default
    )
    {
        DbContext.TeamJoinRequests.Add(teamJoinRequest);
        return Task.CompletedTask;
    }

    public async Task<Team?> GetWithMembersByIdAsync(
        Guid id,
        bool asSplitQuery = false,
        CancellationToken ct = default
    )
    {
        return await BuildQuery<Team>(null, asSplitQuery)
            .Include(t => t.Members)
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync(ct);
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
