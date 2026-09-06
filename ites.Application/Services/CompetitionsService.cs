using ites.Application.Contracts.Competitions;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Enums;
using ites.Core.Exceptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class CompetitionsService(
    ICompetitionsRepository competitionsRepository,
    IUserRepository userRepository
) : ICompetitionsService
{
    public async Task<Guid> AddEntryAsync(
        Guid userId,
        Guid competitionId,
        CompetitionEntryRequest request,
        CancellationToken ct = default
    )
    {
        bool competitionExist = await competitionsRepository.AnyAsync(
            c => c.Id == competitionId,
            ct
        );

        if (!competitionExist)
            throw new NotFoundException("Конкурс не найден.");

        var entry = new CompetitionEntry
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            CompetitionId = competitionId,
            CoverLetter = request.CoverLetter ?? string.Empty,
            Status = RequsetStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        };

        await competitionsRepository.AddCompetitionEntryAsync(entry, ct);
        await competitionsRepository.SaveChangesAsync(ct);

        return entry.Id;
    }

    public async Task<Guid> CreateAsync(
        Guid userId,
        CompetitionRequest request,
        CancellationToken ct = default
    )
    {
        User? organizer =
            await userRepository.GetByIdAsync(userId, ct: ct)
            ?? throw new NotFoundException("Пользователь не найден");

        Competition competition = new()
        {
            Id = Guid.CreateVersion7(),
            ContentInHtml = request.ContentInHtml,
            Title = request.Title,
            Organizers = [organizer],
        };

        await competitionsRepository.CreateAsync(competition, ct);
        await competitionsRepository.SaveChangesAsync(ct);

        return competition.Id;
    }

    public Task DeleteAsync(Guid userId, Guid id, CancellationToken ct = default)
    {
        // TODO: Implement delete competition logic
        throw new NotImplementedException();
    }

    public async Task<CompetitionResponse> GetAsync(Guid id, CancellationToken ct = default)
    {
        var competition =
            await competitionsRepository.GetByIdAsync(
                id,
                c => new CompetitionResponse(c.Id, c.ContentInHtml, c.Title),
                ct: ct
            ) ?? throw new NotFoundException("Конкурс не найден");

        return competition;
    }

    public async Task<CompetitionListResponse> GetAllAsync(
        int page = 1,
        int pageSize = 100,
        CancellationToken ct = default
    )
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var competitions = await competitionsRepository.GetAllAsync(
            c => new CompetitionSummaryResponse(c.Id, c.Title),
            skip: (page - 1) * pageSize,
            take: pageSize,
            ct: ct
        );

        var totalCount = await competitionsRepository.CountAsync(ct);

        return new CompetitionListResponse(competitions, totalCount, page, pageSize);
    }

    public async Task HandleEntryAsync(
        Guid userId,
        Guid entryId,
        HandleCompetitionEntryRequest request,
        CancellationToken ct = default
    )
    {
        var entry =
            await competitionsRepository.GetEntryByIdAsync(entryId, ct)
            ?? throw new NotFoundException("Заявка на конкурс не найдена.");

        if (entry.Status != RequsetStatus.Pending)
            throw new BadRequestException("Эта заявка уже обработана.");

        bool isOrganizer = await competitionsRepository.IsOrganizerAsync(
            entry.CompetitionId,
            userId,
            ct
        );

        if (!isOrganizer)
            throw new ForbiddenException("У вас нет прав для обработки заявок этого конкурса.");

        if (request.Accept)
        {
            entry.Status = RequsetStatus.Accepted;

            await competitionsRepository.AddMemberAsync(entry.CompetitionId, entry.UserId, ct);
        }
        else
        {
            entry.Status = RequsetStatus.Rejected;
        }

        await competitionsRepository.UpdateEntryAsync(entry, ct);
        await competitionsRepository.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(
        Guid userId,
        Guid competitionId,
        UpdateCompetitionRequest request,
        CancellationToken ct = default
    )
    {
        // TODO: Implement update competition logic
        throw new NotImplementedException();
    }
}
