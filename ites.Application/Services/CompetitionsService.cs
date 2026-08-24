using ites.Application.Contracts.Competitions;
using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services;

public sealed class CompetitionsService(
    ICompetitionsRepository competitionsRepository,
    IRequestEntityRepository applicationsRepository,
    IUserRepository userRepository
) : ICompetitionsService
{
    public async Task AddApplicationAsync(
        Guid userId,
        Guid competitionId,
        CancellationToken ct = default
    )
    {
        RequestEntity application = new()
        {
            Id = Guid.CreateVersion7(),
            For = competitionId,
            From = userId,
        };

        await applicationsRepository.CreateForCompetitionAsync(application, ct);
        await competitionsRepository.SaveChangesAsync(ct);
    }

    public async Task<Guid> CreateAsync(
        Guid userId,
        CompetitionRequest request,
        CancellationToken ct = default
    )
    {
        User? organizer =
            await userRepository.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException("Пользователь не найден");

        Competition competition = new()
        {
            Id = Guid.CreateVersion7(),
            ContentInHtml = request.ContentInHtml,
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
                c => new CompetitionResponse(c.Id, c.ContentInHtml),
                ct
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
            c => new CompetitionSummaryResponse(c.Id, c.ContentInHtml),
            (page - 1) * pageSize,
            pageSize,
            ct
        );

        var totalCount = await competitionsRepository.CountAsync(ct);

        return new CompetitionListResponse(competitions, totalCount, page, pageSize);
    }

    public async Task HandleApplicationAsync(Guid id, bool isAccept, CancellationToken ct = default)
    {
        await applicationsRepository.HandleCompetitionAsync(id, isAccept, ct);
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
