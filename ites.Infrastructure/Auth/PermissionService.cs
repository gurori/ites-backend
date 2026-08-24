using ites.Application.Interfaces.Auth;
using ites.Core.Interfaces.Repositories;

namespace ites.Infrastructure.Auth
{
    public sealed class PermissionService(IRoleRepository roleRepository) : IPermissionService
    {
        public Task<HashSet<int>> GetPermissionsIdsAsync(
            string roleName,
            CancellationToken ct = default
        )
        {
            return roleRepository.GetPermissionsIdsAsync(roleName, ct);
        }
    }
}
