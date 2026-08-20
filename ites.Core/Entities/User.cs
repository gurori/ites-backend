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
        public ICollection<Guid> CompetitionsIds { get; set; } = [];
        public ICollection<Guid> OrdersIds { get; set; } = [];
        public ICollection<Guid> ApplicationsForCompetitions { get; set; } = [];
        public ICollection<Guid> ApplicationsForOrders { get; set; } = [];
        public ICollection<Guid> ApplicationsForTeams { get; set; } = [];
        public ICollection<Guid> ApplicationsIds { get; set; } = [];
        public Guid? TeamId { get; set; } = null;
    }
}
