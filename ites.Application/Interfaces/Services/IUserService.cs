using ites.Application.Contracts.Users;

namespace ites.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(string email, string password);
        Task RegisterAsync(string name, string email, string password, string role);
        Task<UserProfileResponse> GetFromTokenAsync(string token);
        Task<Guid> GetIdFromTokenAsync(string token);
        Task<UserProfileResponse> GetAsync(Guid id);
        Task UpdateAsync(Guid id, string lastName, string firstName, string middleName, string description);
    }
}