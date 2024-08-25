using ites.Core.Models;

namespace ites.Application.Contracts.Users
{
    public class MemberResponse(
        Guid id,
        string lastName,
        string firstName,
        string middleName,
        string email,
        string role,
        string description,
        string jobTitle,
        IList<Competition> competitions,
        IList<Competition> applicationsForCompetitions,
        IList<Order> orders,
        IList<Order> applicationsForOrders,
        IList<Team> applicationsForTeams,
        Guid? teamId) //???
    {
        public Guid Id { get; private set; } = id;
        public string LastName { get; private set; } = lastName;
        public string FirstName { get; private set; } = firstName;
        public string MiddleName { get; private set; } = middleName;
        public string Email { get; private set; } = email;
        public string Role { get; private set; } = role;
        public string Description { get; private set; } = description;
        public string JobTitle { get; private set; } = jobTitle;
        public IList<Competition> Competitions { get; private set; } = competitions;
        public IList<Competition> ApplicationsForCompetitions { get; private set; } = applicationsForCompetitions;
        public IList<Order> Orders { get; private set; } = orders;
        public IList<Order> ApplicationsForOrders { get; private set; } = applicationsForOrders;
        public IList<Team> ApplicationsForTeams { get; private set; } = applicationsForTeams;
        public Guid? TeamId { get; private set; } = teamId;
    }
}
