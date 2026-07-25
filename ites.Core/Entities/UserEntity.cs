namespace ites.Core.Entities
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
        public int Coins { get; set; } = 0;
        public IList<Guid> CompetitionsIds { get; set; } = [];
        public IList<Guid> OrdersIds { get; set; } = [];
        public IList<Guid> ApplicationsForCompetitions { get; set; } = [];
        public IList<Guid> ApplicationsForOrders { get; set; } = [];
        public IList<Guid> ApplicationsForTeams { get; set; } = [];
        public IList<Guid> ApplicationsIds { get; set; } = [];
        public Guid? TeamId { get; set; } = null;
    }
}
