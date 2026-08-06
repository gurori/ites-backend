namespace ites.Application.Interfaces.Auth
{
    public interface IPermissionService
    {
        public Task<HashSet<int>> GetPermissionsIdsAsync(string roleName);
    }
}
