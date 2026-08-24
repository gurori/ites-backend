using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository : ICrudRepository<Team>
{
    Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Team, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );
    Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
    Task AddTeamJoinRequestAsync(TeamJoinRequest teamJoinRequest, CancellationToken ct = default);
}
