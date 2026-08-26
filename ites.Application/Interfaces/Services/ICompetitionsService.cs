using ites.Application.Contracts.Competitions;

namespace ites.Application.Interfaces.Services;

public interface ICompetitionsService
{
    Task<Guid> CreateAsync(Guid userId, CompetitionRequest request, CancellationToken ct = default);
    Task<CompetitionResponse> GetAsync(Guid id, CancellationToken ct = default);
    Task<CompetitionListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    );
    Task<Guid> AddEntryAsync(
        Guid userId,
        Guid competitionId,
        CompetitionEntryRequest request,
        CancellationToken ct = default
    );
    Task HandleEntryAsync(
        Guid userId,
        Guid entryId,
        HandleCompetitionEntryRequest request,
        CancellationToken ct = default
    );
    Task UpdateAsync(
        Guid userId,
        Guid competitionId,
        UpdateCompetitionRequest request,
        CancellationToken ct = default
    );
    Task DeleteAsync(Guid userId, Guid id, CancellationToken ct = default);
}
