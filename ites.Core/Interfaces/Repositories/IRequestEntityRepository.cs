using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface IRequestEntityRepository
{
    public Task CreateForCompetitionAsync(
        RequestEntity application,
        CancellationToken ct = default
    );
    public Task HandleCompetitionAsync(Guid id, bool isAccept, CancellationToken ct = default);
    public Task CreateForOrderAsync(RequestEntity application, CancellationToken ct = default);
    public Task HandleOrderAsync(Guid id, bool isAccept, CancellationToken ct = default);
    public Task CreateForTeamAsync(RequestEntity application, CancellationToken ct = default);
    public Task HandleTeamAsync(Guid id, bool isAccept, CancellationToken ct = default);
}
