using Ressource_API.Common.Data;
using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.BackofficeLogLevels.Repositories;

public class BackofficeLogLevelRepository : BaseRepository<BackofficeLogLevel>, IBackofficeLogLevelRepository
{
    public BackofficeLogLevelRepository(ApplicationDbContext context) : base(context)
    {
    }
}
