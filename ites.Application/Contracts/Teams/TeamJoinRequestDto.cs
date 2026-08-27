using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Applications;

public sealed record TeamJoinRequestDto(Guid Id, MemberSummaryResponse FromMember);
