using ites.Application.Interfaces.Auth;
using ites.Core.Interfaces.Repositories;

namespace ites.Infrastructure.Auth
{
    public sealed class PermissionService(IRoleRepository roleRepository) : IPermissionService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<HashSet<int>> GetPermissionsIdsAsync(string roleName)
        {
            return await _roleRepository.GetPermissionsIdsAsync(roleName);
        }
    }
}
