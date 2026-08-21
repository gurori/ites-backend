namespace ites.Core.Entities
{
    public sealed class User : BaseEntity
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int Coins { get; set; } = 0;
        public ICollection<Competition> Competitions { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<RequestEntity> ApplicationsForCompetitions { get; set; } = [];
        public ICollection<RequestEntity> ApplicationsForOrders { get; set; } = [];
        public ICollection<RequestEntity> ApplicationsForTeams { get; set; } = [];
        public ICollection<RequestEntity> Applications { get; set; } = [];
        public Team? Team { get; set; } = null;
        public Guid? TeamId { get; set; } = null;
    }
}
