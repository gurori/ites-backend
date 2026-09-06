using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CompetitionEntry?> GetEntryByIdAsync(
        Guid entryId,
        CancellationToken ct = default
    )
    {
        return await DbContext.CompetitionEntries.FirstOrDefaultAsync(e => e.Id == entryId, ct);
    }

    public Task UpdateEntryAsync(CompetitionEntry entry, CancellationToken ct = default)
    {
        DbContext.CompetitionEntries.Update(entry);
        return Task.CompletedTask;
    }

    public async Task AddMemberAsync(
        Guid competitionId,
        Guid userId,
        CancellationToken ct = default
    )
    {
        var competition = await DbContext
            .Competitions.Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == competitionId, ct);

        if (competition is null || competition.Members.Any(m => m.Id == userId))
            return;

        var userStub = new User { Id = userId };
        DbContext.Users.Attach(userStub);

        competition.Members.Add(userStub);
    }

    public async Task<bool> IsOrganizerAsync(
        Guid userId,
        Guid competitionId,
        CancellationToken ct = default
    )
    {
        return await DbContext.Competitions.AnyAsync(
            c => c.Id == competitionId && c.Organizers.Any(o => o.Id == userId),
            ct
        );
    }
}
