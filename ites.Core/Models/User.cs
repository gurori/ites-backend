namespace ites.Core.Models
{
    public class User(Guid id,
                      string firstName,
                      string email,
                      string passwordHash,
                      string role)
    {
        public Guid Id { get; private set; } = id;
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; }
        public string MiddleName { get; private set; }
        public string Email { get; private set; } = email;
        public string PasswordHash { get; private set; } = passwordHash;
        public string Role { get; private set; } = role;
        public string Description { get; private set; }
        public string JobTitle { get; private set; }
        public IList<Guid> CompetitionsIds { get; private set; } 
        public IList<Guid> OrdersIds { get; private set; }
        public IList<Guid> ApplicationsForCompetitions { get; private set; }
        public IList<Guid> ApplicationsForOrders { get; private set; }
        public IList<Guid> ApplicationsIds { get; private set; }
    }
}