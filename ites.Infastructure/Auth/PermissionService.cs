using ites.Application.Interfaces.Auth;
using ites.Application.Interfaces.Repositories;

namespace ites.Infastructure.Auth
{
    public class PermissionService(IUserRepository userRepository) : IPermissionService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<HashSet<string>> GetPermissionsAsync(string roleName)
        {
            return await _userRepository.GetPermissionsAsync(roleName);
        }
    }
}
