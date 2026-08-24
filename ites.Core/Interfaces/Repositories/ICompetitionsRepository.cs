using ites.Core.Entities;

namespace ites.Core.Interfaces.Repositories;

public interface ICompetitionsRepository : ICrudRepository<Competition>
{
    Task AddCompetitionEntryAsync(
        CompetitionEntry competitionEntry,
        CancellationToken ct = default
    );
}
