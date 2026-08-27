using ites.Application.Contracts.Competitions;

namespace ites.Application.Contracts.Users;

public sealed record OrganizerResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string MiddleName,
    string Email,
    string Role,
    string Description,
    string JobTitle,
    IReadOnlyCollection<CompetitionSummaryResponse> Competitions,
    IReadOnlyCollection<CompetitionEntryResponse> Applications
);
