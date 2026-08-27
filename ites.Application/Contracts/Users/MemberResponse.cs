using ites.Application.Contracts.Applications;
using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;

namespace ites.Application.Contracts.Users;

public sealed record MemberResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string MiddleName,
    string Email,
    string Role,
    string Description,
    string JobTitle,
    Guid? TeamId,
    IReadOnlyCollection<CompetitionSummaryResponse> Competitions,
    IReadOnlyCollection<CompetitionSummaryResponse> ApplicationsForCompetitions,
    IReadOnlyCollection<OrderSummaryResponse> Orders,
    IReadOnlyCollection<OrderSummaryResponse> ApplicationsForOrders,
    IReadOnlyCollection<TeamSummaryResponse> ApplicationsForTeams,
    IReadOnlyCollection<TeamJoinRequestDto> Applications
);
