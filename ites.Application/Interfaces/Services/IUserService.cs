using ites.Application.Contracts.Users;

namespace ites.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginUserResponse> LoginAsync(
            LoginUserRequest request,
            CancellationToken ct = default
        );
        Task RegisterAsync(RegisterUserRequest request, CancellationToken ct = default);
        Task UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid userId, CancellationToken ct = default);
    }
}
