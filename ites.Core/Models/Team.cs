namespace ites.Core.Models
{
    public sealed class Team
    {
        public Team() { }

        public Team(string name, string description, Guid adminId)
        {
            Name = name;
            Description = description;
            AdminId = adminId;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<Guid> MembersIds { get; private set; }
        public Guid AdminId { get; private set; }
    }
}
