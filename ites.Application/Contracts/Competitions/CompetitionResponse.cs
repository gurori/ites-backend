namespace ites.Application.Contracts.Competitions
{
    public record CompetitionResponse(
        Guid Id,
        // string Title,
        // string Description,
        // DateTime StartDate,
        // DateTime EndDate
        string ContentInHtml
        );
}
