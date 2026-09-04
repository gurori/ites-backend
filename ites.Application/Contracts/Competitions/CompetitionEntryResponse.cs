namespace ites.Application.Contracts.Competitions;

public sealed record CompetitionEntryResponse(
    Guid Id,
    Guid UserId,
    string UserFirstName,
    string? CoverLetter
);
