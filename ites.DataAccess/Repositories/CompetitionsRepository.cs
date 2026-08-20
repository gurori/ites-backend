using AutoMapper;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using ites.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class CompetitionsRepository(ItesDbContext context, IMapper mapper)
        : ICompetitionsRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateAsync(Guid userId, Core.Models.Competition competition)
        {
            Core.Entities.User? user = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return false;

            Core.Entities.Competition competitionEntity = new()
            {
                Id = Guid.CreateVersion7(),
                ContentInHtml = competition.ContentInHtml,
                OrganizersIds = [userId],
            };

            await _context.Competitions.AddAsync(competitionEntity);
            user.CompetitionsIds.Add(competitionEntity.Id);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Core.Models.Competition>> GetAllAsync()
        {
            IList<Core.Entities.Competition> competitions = await _context
                .Competitions.AsNoTracking()
                .ToListAsync();

            return _mapper.Map<Core.Models.Competition[]>(competitions);
        }

        public async Task<IList<Core.Models.Competition>> GetAllWithIdAsync(ICollection<Guid> ids)
        {
            IList<Core.Entities.Competition> competitions = await _context
                .Competitions.AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return _mapper.Map<Core.Models.Competition[]>(competitions);
        }

        public async Task<Core.Models.Competition?> GetByIdAsync(Guid id)
        {
            Core.Entities.Competition? competition = await _context
                .Competitions.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return competition is null ? null : _mapper.Map<Core.Models.Competition>(competition);
        }

        public Task UpdateAsync(
            Guid id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate
        )
        {
            throw new NotImplementedException();
        }
    }
}
