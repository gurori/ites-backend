using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ites.Application.Interfaces.Auth;
using ites.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ites.Infrastructure.Auth
{
    public class JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
        : IJwtProvider
    {
        private readonly IPermissionService _permissionService = permissionService;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly JwtOptions _options = options.Value;

        public async Task<string> GenerateTokenAsync(User user)
        {
            List<Claim> claims = [new(ClaimNames.UserId, user.Id.ToString())];

            HashSet<int> permissionsIds = await _permissionService.GetPermissionsIdsAsync(
                user.Role
            );

            claims.Add(new(ClaimNames.Permissions, string.Join(';', permissionsIds)));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.Expires)
            );

            return _tokenHandler.WriteToken(token);
        }
    }
}
