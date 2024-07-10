namespace ites.Application.Interfaces.Auth
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionsAsync(string roleName);
    }
}
