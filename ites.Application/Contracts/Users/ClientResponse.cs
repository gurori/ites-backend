using ites.Application.Contracts.Applications;
using ites.Application.Contracts.Orders;

namespace ites.Application.Contracts.Users;

public sealed record ClientResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string MiddleName,
    string Email,
    string Role,
    string Description,
    string JobTitle,
    IReadOnlyCollection<OrderSummaryResponse> Orders,
    IReadOnlyCollection<OrderBidResponse> Applications
);
