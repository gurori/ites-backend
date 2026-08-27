using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Teams;

public sealed record TeamJoinRequestDto(Guid Id, MemberSummaryResponse FromMember);
