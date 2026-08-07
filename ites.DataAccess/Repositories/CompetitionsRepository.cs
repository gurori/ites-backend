using AutoMapper;
using ites.Core.Models;
using ites.Core.Entities;
using Microsoft.EntityFrameworkCore;
using ites.Core.Interfaces.Repositories;

namespace ites.DataAccess.Repositories
{
    public sealed class CompetitionsRepository(ItesDbContext context, IMapper mapper)
        : ICompetitionsRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CreateAsync(Guid userId, Competition competition)
        {
            UserEntity? user = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return false;

            CompetitionEntity competitionEntity = new()
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

        public async Task<IList<Competition>> GetAllAsync()
        {
            IList<CompetitionEntity> competitions = await _context
                .Competitions.AsNoTracking()
                .ToListAsync();

            return _mapper.Map<Competition[]>(competitions);
        }

        public async Task<IList<Competition>> GetAllWithIdAsync(IList<Guid> ids)
        {
            IList<CompetitionEntity> competitions = await _context
                .Competitions.AsNoTracking()
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return _mapper.Map<Competition[]>(competitions);
        }

        public async Task<Competition?> GetByIdAsync(Guid id)
        {
            CompetitionEntity? competition = await _context
                .Competitions.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return competition is null ? null : _mapper.Map<Competition>(competition);
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
