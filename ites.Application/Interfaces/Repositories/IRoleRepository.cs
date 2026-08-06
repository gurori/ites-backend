namespace ites.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task<HashSet<int>> GetPermissionsIdsAsync(string roleName);
    }
}
