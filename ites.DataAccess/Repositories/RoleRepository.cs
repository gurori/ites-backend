using ites.Core.Enums;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class RoleRepository(ItesDbContext context) : IRoleRepository
    {
        private readonly ItesDbContext _context = context;

        public async Task<HashSet<int>> GetPermissionsIdsAsync(string roleName)
        {
            var permissions = await _context
                .Roles.Include(r => r.Permissions)
                .Where(r => r.Name.ToLower() == roleName)
                .Select(r => r.Permissions)
                .ToArrayAsync();

            return permissions
                .SelectMany(p => p)
                .Select(p => (int)Enum.Parse<Permission>(p.Name))
                .ToHashSet();
        }
    }
}
