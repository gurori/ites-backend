using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class RequestEntityRepository(ItesDbContext context) : IRequestEntityRepository
{
    private readonly ItesDbContext _context = context;

    public async Task CreateForCompetitionAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    )
    {
        bool exists = await _context
            .Applications.AsNoTracking()
            .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For, ct);

        if (exists) return;

        Competition? competition = await _context.Competitions.FirstOrDefaultAsync(
            c => c.Id == requestEntity.For,
            ct
        );

        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (competition is null || fromMember is null)
            return;

        User[] organizers = await _context
            .Users.Where(u => competition.OrganizersIds.Contains(u.Id))
            .ToArrayAsync(ct);

        var newRequest = new RequestEntity
        {
            Id = Guid.CreateVersion7(),
            For = requestEntity.For,
            From = requestEntity.From,
        };

        fromMember.ApplicationsForCompetitions.Add(newRequest.For);
        foreach (User organizer in organizers)
        {
            organizer.ApplicationsIds.Add(newRequest.Id);
        }

        _context.Applications.Add(newRequest);

        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateForOrderAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    )
    {
        Order? order = await _context
            .Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == requestEntity.For, ct);

        if (order is null || !order.IsPublic)
            return;

        bool exists = await _context
            .Applications.AsNoTracking()
            .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For, ct);
        if (exists)
            return;

        User? client = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.ClientId, ct);
        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (client is null || fromMember is null)
            return;

        var newRequest = new RequestEntity
        {
            Id = Guid.CreateVersion7(),
            For = requestEntity.For,
            From = requestEntity.From,
        };

        fromMember.ApplicationsForOrders.Add(newRequest.For);
        client.ApplicationsIds.Add(newRequest.Id);

        _context.Applications.Add(newRequest);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateForTeamAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    )
    {
        Team? team = await _context
            .Teams.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == requestEntity.For, ct);

        if (team is null || team.MembersIds.Count >= 5 || !team.IsPublic)
            return;

        bool exists = await _context
            .Applications.AsNoTracking()
            .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For, ct);
        if (exists)
            return;

        User? admin = await _context.Users.FirstOrDefaultAsync(u => u.Id == team.AdminId, ct);
        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (admin is null || fromMember is null)
            return;

        var newRequest = new RequestEntity
        {
            Id = Guid.CreateVersion7(),
            For = requestEntity.For,
            From = requestEntity.From,
        };

        fromMember.ApplicationsForTeams.Add(newRequest.For);
        admin.ApplicationsIds.Add(newRequest.Id);

        _context.Applications.Add(newRequest);
        await _context.SaveChangesAsync(ct);
    }

    public async Task HandleCompetitionAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        RequestEntity? requestEntity = await _context.Applications.FirstOrDefaultAsync(
            a => a.Id == id,
            ct
        );
        if (requestEntity is null)
            return;

        Competition? competition = await _context.Competitions.FirstOrDefaultAsync(
            c => c.Id == requestEntity.For,
            ct
        );
        User? member = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (competition is null || member is null)
            return;

        IList<User> organizers = await _context
            .Users.Where(u => competition.OrganizersIds.Contains(u.Id))
            .ToListAsync(ct);

        member.ApplicationsForCompetitions.Remove(requestEntity.For);
        foreach (User organizer in organizers)
        {
            organizer.ApplicationsIds.Remove(requestEntity.Id);
        }

        if (isAccept)
        {
            member.CompetitionsIds.Add(requestEntity.For);
            competition.MembersIds.Add(requestEntity.From);
        }

        _context.Applications.Remove(requestEntity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task HandleOrderAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        RequestEntity? requestEntity = await _context.Applications.FirstOrDefaultAsync(
            a => a.Id == id,
            ct
        );
        if (requestEntity is null)
            return;

        Order? order = await _context.Orders.FirstOrDefaultAsync(
            o => o.Id == requestEntity.For,
            ct
        );
        User? member = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (order is null || member is null)
            return;

        User? client = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.ClientId, ct);
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

        _context.Applications.Remove(requestEntity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task HandleTeamAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        RequestEntity? requestEntity = await _context.Applications.FirstOrDefaultAsync(
            a => a.Id == id,
            ct
        );
        if (requestEntity is null)
            return;

        Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == requestEntity.For, ct);
        User? member = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (team is null || member is null || team.MembersIds.Count >= 5)
            return;

        User? admin = await _context.Users.FirstOrDefaultAsync(u => u.Id == team.AdminId, ct);
        if (admin is null)
            return;

        member.ApplicationsForTeams.Remove(requestEntity.For);
        admin.ApplicationsIds.Remove(requestEntity.Id);

        if (isAccept)
        {
            member.TeamId = team.Id;
            team.MembersIds.Add(member.Id);

            if (team.MembersIds.Count >= 5)
                team.IsPublic = false;
        }

        _context.Applications.Remove(requestEntity);
        await _context.SaveChangesAsync(ct);
    }
}
