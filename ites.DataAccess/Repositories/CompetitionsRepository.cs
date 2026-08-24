using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;

namespace ites.DataAccess.Repositories;

public sealed class CompetitionsRepository(ItesDbContext context)
    : CrudRepository<Competition>(context),
        ICompetitionsRepository
{
    public Task AddCompetitionEntryAsync(
        CompetitionEntry competitionEntry,
        CancellationToken ct = default
    )
    {
        DbContext.CompetitionEntries.Add(competitionEntry);
        return Task.CompletedTask;
    }
}
