namespace ites.Application.Interfaces.Auth;

public interface IJwtProvider
{
    public Task<string> GenerateTokenAsync(
        Guid userId,
        string role,
        CancellationToken ct = default
    );
}
