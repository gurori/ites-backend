using AutoMapper;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using ites.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class TeamRepository(ItesDbContext context, IMapper mapper) : ITeamRepository
{
    private readonly ItesDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(Core.Models.Team team)
    {
        Core.Entities.User? admin = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == team.AdminId
        );

        if (admin is null || admin.TeamId is not null)
            return;

        Core.Entities.Team teamEntity = new()
        {
            Id = Guid.CreateVersion7(),
            AdminId = admin.Id,
            Name = team.Name,
            Description = team.Description,
            MembersIds = [admin.Id],
            IsPublic = false,
        };

        admin.TeamId = teamEntity.Id;
        await _context.Teams.AddAsync(teamEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Teams.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task SetIsPublicAsync(Guid id, bool isPublic)
    {
        var teamEntity = await _context.Teams.Where(x => x.Id == id).FirstOrDefaultAsync();
        teamEntity?.IsPublic = isPublic;

        await _context.SaveChangesAsync();
    }

    public async Task<IList<Core.Models.Team>> GetAllPublicAsync()
    {
        IList<Core.Entities.Team> teamEntities = await _context
            .Teams.AsNoTracking()
            .Where(t => t.IsPublic)
            .ToListAsync();

        return _mapper.Map<Core.Models.Team[]>(teamEntities);
    }

    public async Task<Core.Models.Team?> GetByIdAsync(Guid id)
    {
        Core.Entities.Team? teamEntity = await _context
            .Teams.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teamEntity is null)
            return null;

        return _mapper.Map<Core.Models.Team>(teamEntity);
    }

    public async Task<IList<Core.Models.Team>> GetByIdsAsync(IList<Guid> ids)
    {
        IList<Core.Entities.Team> teamEntities = await _context
            .Teams.AsNoTracking()
            .Where(t => ids.Contains(t.Id) && t.IsPublic == true)
            .ToListAsync();

        return _mapper.Map<Core.Models.Team[]>(teamEntities);
    }

    public async Task<IList<Core.Models.Team>> GetAllNotPublicAsync()
    {
        IList<Core.Entities.Team> teamEntities = await _context
            .Teams.AsNoTracking()
            .Where(t => t.IsPublic == false)
            .ToListAsync();

        return _mapper.Map<Core.Models.Team[]>(teamEntities);
    }
}
