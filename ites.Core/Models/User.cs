namespace ites.Core.Models;

public class User(Guid id, string firstName, string email, string passwordHash, string role)
{
    public Guid Id { get; private set; } = id;
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = string.Empty;
    public string MiddleName { get; private set; } = string.Empty;
    public string Email { get; private set; } = email;
    public string PasswordHash { get; private set; } = passwordHash;
    public string Role { get; private set; } = role;
    public string Description { get; private set; } = string.Empty;
    public string JobTitle { get; private set; } = string.Empty;
    public ICollection<Guid> CompetitionsIds { get; private set; } = [];
    public ICollection<Guid> OrdersIds { get; private set; } = [];
    public ICollection<Guid> ApplicationsForCompetitions { get; private set; } = [];
    public ICollection<Guid> ApplicationsForOrders { get; private set; } = [];
    public ICollection<Guid> ApplicationsForTeams { get; private set; } = [];
    public ICollection<Guid> ApplicationsIds { get; private set; } = [];
    public Guid? TeamId { get; private set; }
}
