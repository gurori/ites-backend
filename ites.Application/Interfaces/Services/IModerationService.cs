using ites.Application.Contracts.Moderation;

namespace ites.Application.Interfaces.Services;

public interface IModerationService
{
    Task<ModerationResponse> GetAllAsync(CancellationToken ct = default);
    Task HandleAsync(string type, Guid id, bool accept, CancellationToken ct = default);
}
