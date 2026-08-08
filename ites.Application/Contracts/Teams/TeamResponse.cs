using ites.Application.Contracts.Users;

namespace ites.Application.Contracts.Teams;

public class TeamResponse
{
    public TeamResponse() { }

    public TeamResponse(
        Guid id,
        string name,
        string description,
        IList<UserProfileResponse> members,
        Guid adminId
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Members = members;
        AdminId = adminId;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IList<UserProfileResponse> Members { get; private set; } = [];
    public Guid AdminId { get; private set; }
}
