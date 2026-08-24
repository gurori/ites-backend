using ites.Application.Contracts.Users;

namespace ites.Application.Interfaces.Services;

public interface IUserProfileService
{
    Task<MemberResponse> GetMemberAsync(Guid id, CancellationToken ct = default);
    Task<OrganizerResponse> GetOrganizerAsync(Guid id, CancellationToken ct = default);
    Task<ClientResponse> GetClientAsync(Guid id, CancellationToken ct = default);
}
