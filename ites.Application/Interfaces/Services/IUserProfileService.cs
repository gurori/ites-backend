using ites.Application.Contracts.Users;
using ites.Core.Models;

namespace ites.Application.Interfaces.Services
{
    public interface IUserProfileService
    {
        Task<MemberResponse> GetMemberAsync(string token);
        Task<MemberResponse> GetMemberAsync(Guid id);
        Task<OrganizerResponse> GetOrganizerAsync(string token);
    }
}
