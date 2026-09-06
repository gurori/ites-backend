using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository : ICrudRepository<Team>
{
    Task<Team?> GetWithMembersByIdAsync(
        Guid id,
        bool asSplitQuery = false,
        CancellationToken ct = default
    );
    Task AddMemberToTeamAsync(Guid teamId, Guid userId, CancellationToken ct = default);
    Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);

    Task AddTeamJoinRequestAsync(TeamJoinRequest teamJoinRequest, CancellationToken ct = default);
    Task<TeamJoinRequest?> GetJoinRequestByIdAsync(Guid id, CancellationToken ct = default);
    Task UpdateJoinRequestAsync(TeamJoinRequest joinRequest, CancellationToken ct = default);
}
