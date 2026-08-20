using System.Linq.Expressions;
using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ITeamRepository : IRepository<Team>
{
    public Task<IReadOnlyCollection<Team>> GetAllAsync<T>(
        Expression<Func<Team, T>> selector,
        bool isPublic = true,
        int skip = 0,
        int take = 100,
        CancellationToken ct = default
    );
    public Task SetIsPublicAsync(Guid id, bool isPublic, CancellationToken ct = default);
}
