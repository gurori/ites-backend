using System.Linq.Expressions;
using ites.Application.Contracts.Competitions;
using ites.Application.Contracts.Orders;
using ites.Application.Contracts.Teams;
using ites.Application.Contracts.Users;
using ites.Core.Entities;

namespace ites.Application.Mapping;

public static class UserMapping
{
    public static Expression<Func<User, MemberResponse>> ToMemberResponse = u => new MemberResponse(
        u.Id,
        u.LastName,
        u.FirstName,
        u.MiddleName,
        u.Email,
        u.Role,
        u.Description,
        u.JobTitle,
        u.TeamId,
        u.ParticipatedCompetitions.Select(c => new CompetitionSummaryResponse(
                c.Id,
                c.ContentInHtml
            ))
            .ToArray(),
        u.CompetitionEntries.Select(e => new CompetitionSummaryResponse(
                e.CompetitionId,
                e.Competition.ContentInHtml
            ))
            .ToArray(),
        u.ExecutedOrders.Select(o => new OrderSummaryResponse(
                o.Id,
                o.Title,
                o.Description,
                o.Price,
                o.DeadLine
            ))
            .ToArray(),
        u.OrderBids.Select(b => new OrderSummaryResponse(
                b.OrderId,
                b.Order.Title,
                b.Order.Description,
                b.Order.Price,
                b.Order.DeadLine
            ))
            .ToArray(),
        u.TeamJoinRequests.Select(j => new TeamSummaryResponse(
                j.TeamId,
                j.Team.Name,
                j.Team.Description,
                j.Team.Members.Count
            ))
            .ToArray()
    );

    public static Expression<Func<User, ClientResponse>> ToClientResponse = u => new ClientResponse(
        u.Id,
        u.LastName,
        u.FirstName,
        u.MiddleName,
        u.Email,
        u.Role,
        u.Description,
        u.JobTitle,
        u.CreatedOrders.Select(o => new OrderSummaryResponse(
                o.Id,
                o.Title,
                o.Description,
                o.Price,
                o.DeadLine
            ))
            .ToArray(),
        u.OrderBids.Select(b => new OrderBidResponse(
                b.Id,
                new MemberSummaryResponse(
                    b.UserId,
                    b.User.LastName,
                    b.User.FirstName,
                    b.User.MiddleName,
                    b.User.Description,
                    b.User.JobTitle
                ),
                new OrderSummaryResponse(
                    b.OrderId,
                    b.Order.Title,
                    b.Order.Description,
                    b.Order.Price,
                    b.Order.DeadLine
                )
            ))
            .ToArray()
    );

    public static Expression<Func<User, OrganizerResponse>> ToOrganizerResponse =
        u => new OrganizerResponse(
            u.Id,
            u.LastName,
            u.FirstName,
            u.MiddleName,
            u.Email,
            u.Role,
            u.Description,
            u.JobTitle,
            u.OrganizedCompetitions.Select(c => new CompetitionSummaryResponse(
                    c.Id,
                    c.ContentInHtml
                ))
                .ToArray(),
            u.CompetitionEntries.Select(e => new CompetitionEntryResponse(
                    e.Id,
                    new MemberSummaryResponse(
                        e.UserId,
                        e.User.LastName,
                        e.User.FirstName,
                        e.User.MiddleName,
                        e.User.Description,
                        e.User.JobTitle
                    ),
                    new CompetitionSummaryResponse(e.CompetitionId, e.Competition.ContentInHtml)
                ))
                .ToArray()
        );
}
