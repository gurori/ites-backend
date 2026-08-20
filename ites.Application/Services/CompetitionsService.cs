using ites.Application.Interfaces.Services;
using ites.Core.Entities;
using ites.Core.Exeptions;
using ites.Core.Interfaces.Repositories;

namespace ites.Application.Services
{
    public sealed class CompetitionsService(
        ICompetitionsRepository competitionsRepository,
        IRequestEntityRepository applicationsRepository
    ) : ICompetitionsService
    {
        private readonly ICompetitionsRepository _competitionsRepository = competitionsRepository;
        private readonly IRequestEntityRepository _applicationsRepository = applicationsRepository;

        public async Task AddApplicationAsync(Guid userId, Guid forId)
        {
            RequestEntity application = new(Guid.Empty, userId, forId);
            await _applicationsRepository.CreateForCompetitionAsync(application);
        }

        public async Task CreateAsync(Guid userId, string contentInHtml)
        {
            Competition competition = new(Guid.NewGuid(), contentInHtml);

            bool isCreated = await _competitionsRepository.CreateAsync(userId, competition);

            if (!isCreated)
                throw new NotFoundException("Пользователь не найден");
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Competition> GetAsync(Guid id)
        {
            Competition competition =
                await _competitionsRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Конкурс не найден");

            return competition;
        }

        public async Task<IList<Competition>> GetAsync()
        {
            return await _competitionsRepository.GetAllAsync();
        }

        public async Task<IList<Competition>> GetAsync(ICollection<Guid> ids)
        {
            return await _competitionsRepository.GetAllWithIdAsync(ids);
        }

        public async Task HandleApplicationAsync(Guid id, bool isAccept)
        {
            await _applicationsRepository.HandleCompetitionAsync(id, isAccept);
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
