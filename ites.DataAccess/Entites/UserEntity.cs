namespace ites.DataAccess.Entites
{
    public sealed class UserEntity
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
    }
}