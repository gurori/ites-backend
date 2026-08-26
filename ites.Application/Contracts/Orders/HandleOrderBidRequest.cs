namespace ites.Application.Contracts.Orders;

public sealed record HandleOrderBidRequest(bool Accept, string? CoverLetter);
