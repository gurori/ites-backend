using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IRequestEntityRepository
{
    public Task CreateForCompetitionAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    );
    public Task HandleCompetitionAsync(Guid id, bool isAccept, CancellationToken ct = default);
    public Task CreateForOrderAsync(RequestEntity requestEntity, CancellationToken ct = default);
    public Task HandleOrderAsync(Guid id, bool isAccept, CancellationToken ct = default);
    public Task CreateForTeamAsync(RequestEntity requestEntity, CancellationToken ct = default);
    public Task HandleTeamAsync(Guid id, bool isAccept, CancellationToken ct = default);
}
