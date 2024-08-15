using ites.Application.Contracts.Applications;
using ites.Core.Models;

namespace ites.Application.Contracts.Users
{
    public class ClientResponse(
        Guid id,
        string lastName,
        string firstName,
        string middleName,
        string email,
        string role,
        string description,
        string jobTitle,
        IList<Order> orders,
        IList<OrderApplicationResponse> applications)
    {
        public Guid Id { get; private set; } = id;
        public string LastName { get; private set; } = lastName;
        public string FirstName { get; private set; } = firstName;
        public string MiddleName { get; private set; } = middleName;
        public string Email { get; private set; } = email;
        public string Role { get; private set; } = role;
        public string Description { get; private set; } = description;
        public string JobTitle { get; private set; } = jobTitle;
        public IList<Order> Orders { get; private set; } = orders;
        public IList<OrderApplicationResponse> Applications { get; private set; } = applications;
    }
}
