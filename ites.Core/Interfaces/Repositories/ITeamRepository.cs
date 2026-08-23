using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository : ICrudRepository<Team>
{
    public Task<IReadOnlyCollection<T>> GetByVisibilityAsync<T>(
        Expression<Func<Team, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );
    public Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
}
