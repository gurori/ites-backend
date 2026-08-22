namespace ites.Application.Contracts.Users;

public sealed record MemberSummaryResponse(
    Guid Id,
    string LastName,
    string FirstName,
    string MiddleName,
    string Description,
    string JobTitle
);
