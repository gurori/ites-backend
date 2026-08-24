using ites.Application.Contracts.Teams;

namespace ites.Application.Interfaces.Services;

public interface ITeamService
{
    Task<Guid> CreateAsync(Guid userId, TeamRequest request, CancellationToken ct = default);
    Task<TeamResponse> GetAsync(Guid id, CancellationToken ct = default);
    Task<TeamListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    );
    Task AddApplicationAsync(Guid userId, Guid teamId, CancellationToken ct = default);
    Task HandleApplicationAsync(Guid id, bool isAccept, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
