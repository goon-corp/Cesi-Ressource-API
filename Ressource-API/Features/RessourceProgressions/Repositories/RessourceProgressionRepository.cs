using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RessourceProgressions.Repositories;

public class RessourceProgressionRepository : BaseRepository<RessourceProgression>, IRessourceProgressionRepository
{
    public RessourceProgressionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
