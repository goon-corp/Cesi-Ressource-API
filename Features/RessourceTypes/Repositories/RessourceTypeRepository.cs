using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RessourceTypes.Repositories;

public class RessourceTypeRepository : BaseRepository<RessourceType>, IRessourceTypeRepository
{
    public RessourceTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
