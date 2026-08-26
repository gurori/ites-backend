using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ICompetitionsRepository : ICrudRepository<Competition>
{
    Task AddCompetitionEntryAsync(
        CompetitionEntry competitionEntry,
        CancellationToken ct = default
    );
    Task<CompetitionEntry?> GetEntryByIdAsync(Guid entryId, CancellationToken ct = default);
    Task UpdateEntryAsync(CompetitionEntry entry, CancellationToken ct = default);
    Task AddMemberAsync(Guid competitionId, Guid userId, CancellationToken ct = default);
    Task<bool> IsOrganizerAsync(Guid userId, Guid competitionId, CancellationToken ct = default);
}
