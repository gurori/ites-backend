namespace ites.Application.Contracts.Competitions
{
    public record CompetitionRequest(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate
        );
}
