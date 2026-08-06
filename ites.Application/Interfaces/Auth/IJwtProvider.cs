using ites.Core.Models;

namespace ites.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public Task<string> GenerateTokenAsync(User user);
    }
}
