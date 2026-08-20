using ites.Application.Contracts.Moderation;

namespace ites.Application.Interfaces.Services;

public interface IModersService
{
    Task<ModerationResponse> GetAllAsync();
    Task HandleAsync(string type, Guid id, bool accept);
}
