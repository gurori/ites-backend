using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications;

public sealed record TeamApplicationResponse(Guid Id, MemberSummaryResponse FromMember);
