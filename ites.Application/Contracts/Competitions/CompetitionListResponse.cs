namespace ites.Application.Contracts.Competitions;

public sealed record CompetitionListResponse(
    IReadOnlyCollection<CompetitionSummaryResponse> Competitions,
    int TotalCount,
    int Page,
    int PageSize
);
