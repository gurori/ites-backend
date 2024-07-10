namespace ites.DataAccess.Entites
{
    public sealed class RoleEntity 
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<PermissionEntity> Permissions { get; set; } = [];
    }
}
