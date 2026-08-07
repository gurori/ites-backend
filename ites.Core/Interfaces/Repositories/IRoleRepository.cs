namespace ites.Core.Interfaces.Repositories;

public interface IRoleRepository
{
    public Task<HashSet<int>> GetPermissionsIdsAsync(string roleName);
}
