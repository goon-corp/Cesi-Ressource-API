using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RessourceStatuses.Repositories;

public class RessourceStatusRepository : BaseRepository<RessourceStatus>, IRessourceStatusRepository
{
    public RessourceStatusRepository(ApplicationDbContext context) : base(context)
    {
    }
}
