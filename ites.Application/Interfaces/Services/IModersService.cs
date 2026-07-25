using ites.Application.Contracts.Moders;

namespace ites.Application.Interfaces.Services;

public interface IModersService
    {
        Task<ModerResponse> GetAllAsync();
        Task HandleAsync(string type, Guid id, bool accept);
    }
