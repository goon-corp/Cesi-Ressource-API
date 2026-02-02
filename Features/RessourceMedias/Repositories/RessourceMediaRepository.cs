using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RessourceMedias.Repositories;

public class RessourceMediaRepository : BaseRepository<RessourceMedia>, IRessourceMediaRepository
{
    public RessourceMediaRepository(ApplicationDbContext context) : base(context)
    {
    }
}
