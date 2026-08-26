namespace ites.Application.Contracts.Orders;

public sealed record OrderBidRequest(decimal? ProposedPrice, string? CoverLetter);
