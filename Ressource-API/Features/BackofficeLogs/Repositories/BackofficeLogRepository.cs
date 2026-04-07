using Ressource_API.Common.Data;
using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.BackofficeLogs.Repositories;

public class BackofficeLogRepository : BaseRepository<BackofficeLog>, IBackofficeLogRepository
{
    public BackofficeLogRepository(ApplicationDbContext context) : base(context)
    {
    }
}
