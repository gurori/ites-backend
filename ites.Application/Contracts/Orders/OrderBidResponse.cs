namespace ites.Application.Contracts.Orders;

public sealed record OrderBidResponse(
    Guid Id,
    Guid UserId,
    string UserFirstName,
    decimal? ProposedPrice,
    string? CoverLetter
);
