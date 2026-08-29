using ites.Core.Entities;
using ites.Core.Interfaces.Repositories;

namespace ites.DataAccess.Repositories;

public sealed class FileEntityRepository(ItesDbContext dbContext)
    : BaseRepository<FileEntity>(dbContext),
        IFileEntityRepository { }
