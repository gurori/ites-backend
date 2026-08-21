using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;

namespace ites.DataAccess.Repositories
{
    public sealed class CompetitionsRepository(ItesDbContext context)
        : BaseRepository<Competition>(context),
            ICompetitionsRepository { }
}
