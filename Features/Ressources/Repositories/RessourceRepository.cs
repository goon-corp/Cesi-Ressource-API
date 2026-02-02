using Ressource_API.Common.Data;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Ressources.Repositories;

public class RessourceRepository : BaseRepository<Ressource>, IRessourceRepository
{
    public RessourceRepository(ApplicationDbContext context) : base(context)
    {
    }
}
