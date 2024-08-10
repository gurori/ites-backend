using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class ApplicationsRepository(
        ItesDbContext context,
        IMapper mapper)
            : IApplicationsRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task CreateForCompetitionAsync(Core.Models.Application application)
        {
            ApplicationEntity? applRequest1 = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.From == application.From);
            ApplicationEntity? applRequest2 = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.For == application.For);

            if (applRequest1 is not null && applRequest2 is not null)
                if (applRequest1.Id == applRequest2.Id) return;

            ApplicationEntity applicationEntity = new()
            {
                Id = Guid.NewGuid(),
                For = application.For,
                From = application.From,
            };

            UserEntity? fromMemeber = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (fromMemeber is null) return;

            CompetitionEntity? forCompetition = await _context.Competitions
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == application.For);
            if (forCompetition is null) return;

            fromMemeber.ApplicationsForCompetitions
                .Add(applicationEntity.For);

            IList<UserEntity> organizers = await _context.Users
                .AsNoTracking()
                .Where(u => forCompetition.OrganizersIds
                    .Contains(u.Id))
                .ToListAsync();

            foreach (UserEntity organizer in organizers) 
                organizer.ApplicationsIds
                    .Add(applicationEntity.Id);

            organizers.Add(fromMemeber);
            await _context.Applications.AddAsync(applicationEntity);
            _context.Users.UpdateRange(organizers);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Core.Models.Application>> GetManyByIdsAsync(IList<Guid> ids)
        {
            IList<ApplicationEntity> applicationEntities = await _context.Applications
                .AsNoTracking()
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();

            return _mapper.Map<Core.Models.Application[]>(applicationEntities);
        }

        public async Task HandleCompetitionAsync(Guid id, bool isAccept)
        {
            ApplicationEntity? application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null) return;

            CompetitionEntity? competition = await _context.Competitions
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == application.For);
            if (competition is null) return;

            UserEntity? member = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null) return;

            IList<UserEntity> organizers = await _context.Users
                .AsNoTracking()
                .Where(u => competition.OrganizersIds.Contains(u.Id))
                .ToListAsync();

            member.ApplicationsForCompetitions
                .Remove(application.For);
            foreach (UserEntity organizer in organizers)
                organizer.ApplicationsIds.Remove(application.Id);

            if (isAccept)
            {
                member.CompetitionsIds.Add(application.For);
                competition.MembersIds.Add(application.From);
            }

            organizers.Add(member);
            _context.Users.UpdateRange(organizers);
            _context.Competitions.Update(competition);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }
    }
}
