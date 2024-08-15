using ites.Application.Contracts.Users;
using ites.Core.Models;

namespace ites.Application.Interfaces.Services
{
    public interface IUserProfileService
    {
        Task<MemberResponse> GetMemberAsync(string token);
        Task<MemberResponse> GetMemberAsync(Guid id);
        Task<OrganizerResponse> GetOrganizerAsync(string token);
        Task<OrganizerResponse> GetOrganizerAsync(Guid id);
        Task<ClientResponse> GetClientAsync(string token);
        Task<ClientResponse> GetClientAsync(Guid id);
    }
}
