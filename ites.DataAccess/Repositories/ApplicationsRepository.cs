using AutoMapper;
using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories
{
    public sealed class ApplicationsRepository(ItesDbContext context, IMapper mapper)
        : IRequestEntityRepository
    {
        private readonly ItesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task CreateForCompetitionAsync(RequestEntity requestEntity)
        {
            try
            {
                var result = await CreateApplicationAsync(requestEntity);
                RequestEntity requestEntityEntity = result.Item1;
                User fromMemeber = result.Item2;

                Competition? forCompetition = await _context
                    .Competitions.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == requestEntity.For);
                if (forCompetition is null)
                    return;

                fromMemeber.ApplicationsForCompetitions.Add(requestEntityEntity.For);

                IList<User> organizers = await _context
                    .Users.AsNoTracking()
                    .Where(u => forCompetition.OrganizersIds.Contains(u.Id))
                    .ToListAsync();

                foreach (User organizer in organizers)
                    organizer.ApplicationsIds.Add(requestEntityEntity.Id);

                organizers.Add(fromMemeber);
                await _context.Applications.AddAsync(requestEntityEntity);
                _context.Users.UpdateRange(organizers);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task CreateForOrderAsync(RequestEntity requestEntity)
        {
            try
            {
                Order? forOrder = await _context
                    .Orders.AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == requestEntity.For);
                if (forOrder is null || !forOrder.IsPublic)
                    return;

                var result = await CreateApplicationAsync(requestEntity);
                RequestEntity requestEntityEntity = result.Item1;
                User fromMemeber = result.Item2;

                fromMemeber.ApplicationsForOrders.Add(requestEntityEntity.For);

                User? client = await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forOrder.ClientId);
                if (client is null)
                    return;

                client.ApplicationsIds.Add(requestEntityEntity.Id);

                await _context.Applications.AddAsync(requestEntityEntity);
                _context.Users.UpdateRange([client, fromMemeber]);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task CreateForTeamAsync(RequestEntity requestEntity)
        {
            try
            {
                Team? forTeam = await _context
                    .Teams.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == requestEntity.For);
                if (forTeam is null || forTeam.MembersIds.Count >= 5 || forTeam.IsPublic == false)
                    return;

                var result = await CreateApplicationAsync(requestEntity);
                RequestEntity requestEntityEntity = result.Item1;
                User fromMemeber = result.Item2;

                fromMemeber.ApplicationsForTeams.Add(requestEntityEntity.For);

                User? admin = await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == forTeam.AdminId);
                if (admin is null)
                    return;

                admin.ApplicationsIds.Add(requestEntityEntity.Id);

                await _context.Applications.AddAsync(requestEntityEntity);
                _context.Users.UpdateRange([admin, fromMemeber]);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }

        public async Task<IList<RequestEntity>> GetAsync(ICollection<Guid> ids)
        {
            IList<RequestEntity> requestEntityEntities = await _context
                .Applications.AsNoTracking()
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();

            return _mapper.Map<RequestEntity[]>(requestEntityEntities);
        }

        public async Task HandleCompetitionAsync(Guid id, bool isAccept)
        {
            RequestEntity? requestEntity = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (requestEntity is null)
                return;

            Competition? competition = await _context
                .Competitions.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == requestEntity.For);
            if (competition is null)
                return;

            User? member = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == requestEntity.From);
            if (member is null)
                return;

            IList<User> organizers = await _context
                .Users.AsNoTracking()
                .Where(u => competition.OrganizersIds.Contains(u.Id))
                .ToListAsync();

            member.ApplicationsForCompetitions.Remove(requestEntity.For);
            foreach (User organizer in organizers)
                organizer.ApplicationsIds.Remove(requestEntity.Id);

            if (isAccept)
            {
                member.CompetitionsIds.Add(requestEntity.For);
                competition.MembersIds.Add(requestEntity.From);
            }

            organizers.Add(member);
            _context.Users.UpdateRange(organizers);
            _context.Competitions.Update(competition);
            _context.Applications.Remove(requestEntity);
            await _context.SaveChangesAsync();
        }

        public async Task HandleOrderAsync(Guid id, bool isAccept)
        {
            RequestEntity? requestEntity = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (requestEntity is null)
                return;

            Order? order = await _context
                .Orders.AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == requestEntity.For);
            if (order is null)
                return;

            User? member = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == requestEntity.From);
            if (member is null)
                return;

            User? client = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == order.ClientId);
            if (client is null)
                return;

            member.ApplicationsForOrders.Remove(requestEntity.For);
            client.ApplicationsIds.Remove(requestEntity.Id);

            if (isAccept)
            {
                member.OrdersIds.Add(requestEntity.For);
                order.MemberId = requestEntity.From;
                order.IsPublic = false;
            }

            _context.Users.UpdateRange([member, client]);
            _context.Orders.Update(order);
            _context.Applications.Remove(requestEntity);
            await _context.SaveChangesAsync();
        }

        public async Task HandleTeamAsync(Guid id, bool isAccept)
        {
            RequestEntity? requestEntity = await _context
                .Applications.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (requestEntity is null)
                return;

            Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == requestEntity.For);
            if (team is null || team.MembersIds.Count >= 5)
                return;

            User? member = await _context.Users.FirstOrDefaultAsync(u =>
                u.Id == requestEntity.From
            );
            if (member is null)
                return;

            User? admin = await _context
                .Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == team.AdminId);
            if (admin is null)
                return;

            member.ApplicationsForTeams.Remove(requestEntity.For);
            admin.ApplicationsIds.Remove(requestEntity.Id);

            if (isAccept)
            {
                member.TeamId = team.Id;
                team.MembersIds.Add(member.Id);
            }

            if (team.MembersIds.Count >= 5)
                team.IsPublic = false;

            _context.Users.UpdateRange([member, admin]);
            _context.Teams.Update(team);
            _context.Applications.Remove(requestEntity);
            await _context.SaveChangesAsync();
        }

        private async Task<(RequestEntity, User)> CreateApplicationAsync(
            RequestEntity requestEntity
        )
        {
            bool isApplicationExist = await _context
                .Applications.AsNoTracking()
                .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For);

            if (isApplicationExist)
                throw new Exception();

            RequestEntity requestEntityEntity = new()
            {
                Id = Guid.CreateVersion7(),
                For = requestEntity.For,
                From = requestEntity.From,
            };

            User? fromMemeber =
                await _context
                    .Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == requestEntity.From) ?? throw new Exception();

            return (requestEntityEntity, fromMemeber);
        }
    }
}
