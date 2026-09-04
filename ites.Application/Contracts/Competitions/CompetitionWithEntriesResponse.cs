namespace ites.Application.Contracts.Competitions;

public sealed record CompetitionWithEntriesResponse(
    Guid Id,
    string Title,
    IReadOnlyCollection<CompetitionEntryResponse> Entries
);