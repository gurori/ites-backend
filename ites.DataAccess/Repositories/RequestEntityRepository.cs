using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ites.DataAccess.Repositories;

public sealed class RequestEntityRepository(ItesDbContext context)
    : BaseRepository<RequestEntity>(context),
        IRequestEntityRepository
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
        if (exists)
            return;

        Competition? competition = await _context
            .Competitions.Include(c => c.Organizers)
            .FirstOrDefaultAsync(c => c.Id == requestEntity.For, ct);

        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );

        if (competition is null || fromMember is null)
            return;

        fromMember.CompetitionEntries.Add(requestEntity);

        foreach (User organizer in competition.Organizers)
        {
            organizer.Applications.Add(requestEntity);
        }

        _context.Applications.Add(requestEntity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateForOrderAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    )
    {
        bool exists = await _context
            .Applications.AsNoTracking()
            .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For, ct);
        if (exists)
            return;

        Order? order = await _context
            .Orders.Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.Id == requestEntity.For, ct);

        if (order is null || !order.IsPublic)
            return;

        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );
        if (fromMember is null)
            return;

        var newRequest = new RequestEntity
        {
            Id = Guid.CreateVersion7(),
            For = requestEntity.For,
            From = requestEntity.From,
        };

        fromMember.OrderBids.Add(newRequest);
        order.Client.Applications.Add(newRequest);

        _context.Applications.Add(newRequest);
        await _context.SaveChangesAsync(ct);
    }

    public async Task CreateForTeamAsync(
        RequestEntity requestEntity,
        CancellationToken ct = default
    )
    {
        bool exists = await _context
            .Applications.AsNoTracking()
            .AnyAsync(a => a.From == requestEntity.From && a.For == requestEntity.For, ct);
        if (exists)
            return;

        Team? team = await _context
            .Teams.Include(t => t.Admin)
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == requestEntity.For, ct);

        if (team is null || team.Members.Count >= 5 || !team.IsPublic)
            return;

        User? fromMember = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == requestEntity.From,
            ct
        );
        if (fromMember is null)
            return;

        var newRequest = new RequestEntity
        {
            Id = Guid.CreateVersion7(),
            For = requestEntity.For,
            From = requestEntity.From,
        };

        fromMember.TeamJoinRequests.Add(newRequest);
        team.Admin.Applications.Add(newRequest);

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

        if (isAccept)
        {
            Competition? competition = await _context
                .Competitions.Include(c => c.Members)
                .FirstOrDefaultAsync(c => c.Id == requestEntity.For, ct);

            User? member = await _context.Users.FirstOrDefaultAsync(
                u => u.Id == requestEntity.From,
                ct
            );

            if (competition != null && member != null)
            {
                competition.Members.Add(member);
            }
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

        if (isAccept)
        {
            Order? order = await _context.Orders.FirstOrDefaultAsync(
                o => o.Id == requestEntity.For,
                ct
            );
            if (order != null)
            {
                order.MemberId = requestEntity.From;
                order.IsPublic = false;
            }
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

        if (isAccept)
        {
            Team? team = await _context
                .Teams.Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == requestEntity.For, ct);

            User? member = await _context.Users.FirstOrDefaultAsync(
                u => u.Id == requestEntity.From,
                ct
            );

            if (team != null && member != null && team.Members.Count < 5)
            {
                team.Members.Add(member);
                member.TeamId = team.Id;

                if (team.Members.Count >= 5)
                {
                    team.IsPublic = false;
                }
            }
        }

        _context.Applications.Remove(requestEntity);
        await _context.SaveChangesAsync(ct);
    }
}
