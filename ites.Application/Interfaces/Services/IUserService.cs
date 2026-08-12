using ites.Application.Contracts.Users;

namespace ites.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginUserResponse> LoginAsync(string email, string password);
        Task RegisterAsync(string name, string email, string password, string role);
        Task<UserProfileResponse> GetAsync(Guid id);
        Task UpdateAsync(
            Guid id,
            string lastName,
            string firstName,
            string middleName,
            string description,
            string? jobTitle
        );
        Task<string> GetRoleAsync(Guid userId);
        Task<IList<UserProfileResponse>> GetManyAsync(IList<Guid> ids);
        Task DeleteAsync(Guid userId);
    }
}
