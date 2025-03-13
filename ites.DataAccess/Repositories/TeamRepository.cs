using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.Core.Models;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class TeamRepository(ItesDbContext context, IMapper mapper) : ITeamRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task CreateAsync(Team team)
        {
            UserEntity? admin = await _context.Users.FirstOrDefaultAsync(u => u.Id == team.AdminId);

            if (admin is null || admin.TeamId is not null)
                return;

            TeamEntity teamEntity = new()
            {
                Id = Guid.NewGuid(),
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
            teamEntity.IsPublic = isPublic;

            await _context.SaveChangesAsync();
        }

        public async Task<IList<Team>> GetAllPublicAsync()
        {
            IList<TeamEntity> teamEntities = await _context
                .Teams.AsNoTracking()
                .Where(t => t.IsPublic)
                .ToListAsync();

            return _mapper.Map<Team[]>(teamEntities);
        }

        public async Task<Team?> GetByIdAsync(Guid id)
        {
            TeamEntity? teamEntity = await _context
                .Teams.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teamEntity is null)
                return null;

            return _mapper.Map<Team>(teamEntity);
        }

        public async Task<IList<Team>> GetByIdsAsync(IList<Guid> ids)
        {
            IList<TeamEntity> teamEntities = await _context
                .Teams.AsNoTracking()
                .Where(t => ids.Contains(t.Id) && t.IsPublic == true)
                .ToListAsync();

            return _mapper.Map<Team[]>(teamEntities);
        }

        public async Task<IList<Team>> GetAllNotPublicAsync()
        {
            IList<TeamEntity> teamEntities = await _context
                .Teams.AsNoTracking()
                .Where(t => t.IsPublic == false)
                .ToListAsync();

            return _mapper.Map<Team[]>(teamEntities);
        }
    }
}
