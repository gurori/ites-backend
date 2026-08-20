using AutoMapper;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class ApplicationsRepository(ItesDbContext context, IMapper mapper)
        : IApplicationsRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task CreateForCompetitionAsync(Core.Models.Application application)
        {
            try
            {
                var result = await CreateApplicationAsync(application);
                Application applicationEntity = result.Item1;
                User fromMemeber = result.Item2;

                Competition? forCompetition = await _context
                    .Competitions.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == application.For);
                if (forCompetition is null)
                    return;

                fromMemeber.ApplicationsForCompetitions.Add(applicationEntity.For);

                IList<User> organizers = await _context
                    .Users.AsNoTracking()
                    .Where(u => forCompetition.OrganizersIds.Contains(u.Id))
                    .ToListAsync();

                foreach (User organizer in organizers)
                    organizer.ApplicationsIds.Add(applicationEntity.Id);

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
                Order? forOrder = await _context
                    .Orders.AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == application.For);
                if (forOrder is null || !forOrder.IsPublic)
                    return;

                var result = await CreateApplicationAsync(application);
                Application applicationEntity = result.Item1;
                User fromMemeber = result.Item2;

                fromMemeber.ApplicationsForOrders.Add(applicationEntity.For);

                User? client = await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forOrder.ClientId);
                if (client is null)
                    return;

                client.ApplicationsIds.Add(applicationEntity.Id);

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
                Team? forTeam = await _context
                    .Teams.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == application.For);
                if (forTeam is null || forTeam.MembersIds.Count >= 5 || forTeam.IsPublic == false)
                    return;

                var result = await CreateApplicationAsync(application);
                Application applicationEntity = result.Item1;
                User fromMemeber = result.Item2;

                fromMemeber.ApplicationsForTeams.Add(applicationEntity.For);

                User? admin = await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forTeam.AdminId);
                if (admin is null)
                    return;

                admin.ApplicationsIds.Add(applicationEntity.Id);

                await _context.Applications.AddAsync(applicationEntity);
                _context.Users.UpdateRange([admin, fromMemeber]);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task<IList<Core.Models.Application>> GetAsync(ICollection<Guid> ids)
        {
            IList<Application> applicationEntities = await _context
                .Applications.AsNoTracking()
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();

            return _mapper.Map<Core.Models.Application[]>(applicationEntities);
        }

        public async Task HandleCompetitionAsync(Guid id, bool isAccept)
        {
            Application? application = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null)
                return;

            Competition? competition = await _context
                .Competitions.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == application.For);
            if (competition is null)
                return;

            User? member = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null)
                return;

            IList<User> organizers = await _context
                .Users.AsNoTracking()
                .Where(u => competition.OrganizersIds.Contains(u.Id))
                .ToListAsync();

            member.ApplicationsForCompetitions.Remove(application.For);
            foreach (User organizer in organizers)
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
            Application? application = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null)
                return;

            Order? order = await _context
                .Orders.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == application.For);
            if (order is null)
                return;

            User? member = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null)
                return;

            User? client = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == order.ClientId);
            if (client is null)
                return;

            member.ApplicationsForOrders.Remove(application.For);
            client.ApplicationsIds.Remove(application.Id);

            if (isAccept)
            {
                member.OrdersIds.Add(application.For);
                order.MemberId = application.From;
                order.IsPublic = false;
            }

            _context.Users.UpdateRange([member, client]);
            _context.Orders.Update(order);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task HandleTeamAsync(Guid id, bool isAccept)
        {
            Application? application = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application is null)
                return;

            Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == application.For);
            if (team is null || team.MembersIds.Count >= 5)
                return;

            User? member = await _context.Users.FirstOrDefaultAsync(u => u.Id == application.From);
            if (member is null)
                return;

            User? admin = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == team.AdminId);
            if (admin is null)
                return;

            member.ApplicationsForTeams.Remove(application.For);
            admin.ApplicationsIds.Remove(application.Id);

            if (isAccept)
            {
                member.TeamId = team.Id;
                team.MembersIds.Add(member.Id);
            }

            if (team.MembersIds.Count >= 5)
                team.IsPublic = false;

            _context.Users.UpdateRange([member, admin]);
            _context.Teams.Update(team);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
        }

        private async Task<(Application, User)> CreateApplicationAsync(
            Core.Models.Application application
        )
        {
            bool isApplicationExist = await _context
                .Applications.AsNoTracking()
                .AnyAsync(a => a.From == application.From && a.For == application.For);

            if (isApplicationExist)
                throw new Exception();

            Application applicationEntity = new()
            {
                Id = Guid.CreateVersion7(),
                For = application.For,
                From = application.From,
            };

            User? fromMemeber =
                await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == application.From) ?? throw new Exception();

            return (applicationEntity, fromMemeber);
        }
    }
}
