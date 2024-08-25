using AutoMapper;
using ites.Application.Interfaces.Repositories;
using ites.Core.Models;
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
            try
            {
                var result = await CreateApplicationAsync(application);
                ApplicationEntity applicationEntity = result.Item1;
                UserEntity fromMemeber = result.Item2;

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
            catch
            {
                return;
            }
        }

        public async Task CreateForOrderAsync(Core.Models.Application application)
        {
            try
            {
                var result = await CreateApplicationAsync(application);
                ApplicationEntity applicationEntity = result.Item1;
                UserEntity fromMemeber = result.Item2;

                OrderEntity? forOrder = await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == application.For);
                if (forOrder is null) return;

                fromMemeber.ApplicationsForOrders
                    .Add(applicationEntity.For);

                UserEntity? client = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forOrder.ClientId);
                if (client is null) return;

                client.ApplicationsIds
                    .Add(applicationEntity.Id);

                await _context.Applications.AddAsync(applicationEntity);
                _context.Users.UpdateRange([client, fromMemeber]);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task CreateForTeamAsync(Core.Models.Application application)
        {
            try
            {
                var result = await CreateApplicationAsync(application);
                ApplicationEntity applicationEntity = result.Item1;
                UserEntity fromMemeber = result.Item2;

                TeamEntity? forTeam = await _context.Teams
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == application.For);
                if (forTeam is null || forTeam.MembersIds.Count >= 5) return;

                fromMemeber.ApplicationsForTeams
                    .Add(applicationEntity.For);

                UserEntity? admin = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forTeam.AdminId);
                if (admin is null) return;

                admin.ApplicationsIds
                    .Add(applicationEntity.Id);

                await _context.Applications.AddAsync(applicationEntity);
                _context.Users.UpdateRange([admin, fromMemeber]);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task<IList<Core.Models.Application>> GetAsync(IList<Guid> ids)
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

        public async Task HandleOrderAsync(Guid id, bool isAccept)
        {
            ApplicationEntity? application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null) return;

            OrderEntity? order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == application.For);
            if (order is null) return;

            UserEntity? member = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null) return;

            UserEntity? client = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == order.ClientId);
            if (client is null) return;

            member.ApplicationsForOrders
                .Remove(application.For);
            client.ApplicationsIds
                .Remove(application.Id);

            if (isAccept)
            {
                member.OrdersIds.Add(application.For);
                order.MemberId = application.From;
            }

            _context.Users.UpdateRange([member, client]);
            _context.Orders.Update(order);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task HandleTeamAsync(Guid id, bool isAccept)
        {
            ApplicationEntity? application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null) return;

            TeamEntity? team = await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == application.For);
            if (team is null || team.MembersIds.Count >= 5) return;

            UserEntity? member = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null) return;

            UserEntity? admin = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == team.AdminId);
            if (admin is null) return;

            member.ApplicationsForTeams
                .Remove(application.For);
            admin.ApplicationsIds
                .Remove(application.Id);

            if (isAccept)
            {
                member.TeamId = team.Id;
                team.MembersIds.Add(member.Id);
            }

            _context.Users.UpdateRange([member, admin]);
            _context.Teams.Update(team);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        private async Task<(ApplicationEntity, UserEntity)> CreateApplicationAsync(
            Core.Models.Application application)
        {
            bool isApplicationExist = await _context.Applications
                .AsNoTracking()
                .AnyAsync(a => a.From == application.From 
                            && a.For == application.For);

            if (isApplicationExist)
                throw new Exception();

            ApplicationEntity applicationEntity = new()
            {
                Id = Guid.NewGuid(),
                For = application.For,
                From = application.From,
            };

            UserEntity? fromMemeber = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From)
                    ?? throw new Exception();

            return (applicationEntity, fromMemeber);
        }
    }
}
