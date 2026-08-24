namespace ites.Application.Contracts.Teams;

public sealed record TeamListResponse(
    IReadOnlyCollection<TeamSummaryResponse> Teams,
    int TotalCount,
    int Page,
    int PageSize
);
