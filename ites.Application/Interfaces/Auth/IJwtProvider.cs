using System.Security.Claims;
using ites.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace ites.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public Task<string> GenerateTokenAsync(User user);
        public ClaimsPrincipal ValidateToken(string token);
        public Task<TokenValidationResult> ValidateTokenAsync(string token);
    }
}
