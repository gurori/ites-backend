using ites.Core.Enums;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class RoleRepository(ItesDbContext context) : IRoleRepository
    {
        public async Task<HashSet<int>> GetPermissionsIdsAsync(
            string roleName,
            CancellationToken ct = default
        )
        {
            var permissions = await context
                .Roles.Where(r => r.Name.ToLower() == roleName)
                .Select(r => r.Permissions)
                .ToArrayAsync(ct);

            return permissions
                .SelectMany(p => p)
                .Select(p => (int)Enum.Parse<Permission>(p.Name))
                .ToHashSet();
        }
    }
}
