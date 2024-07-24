namespace ites.Core.Models
{
    public class User(Guid id,
                      string firstName,
                      string email,
                      string passwordHash,
                      string role)//,
                      //string lastName,
                      //string middleName,
                      //string description)
    {
        public Guid Id { get; private set; } = id;
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; }// = lastName;
        public string MiddleName { get; private set; }// = middleName;
        public string Email { get; private set; } = email;
        public string PasswordHash { get; private set; } = passwordHash;
        public string Role { get; private set; } = role;
        public string Description { get; private set; }// = description;
        public string JobTitle { get; private set; }
    }
}