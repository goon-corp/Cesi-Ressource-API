using Ressource_API.Common.Data;
using Ressource_API.Features.Regions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Regions.Repositories;

public class RegionRepository : BaseRepository<Region>, IRegionRepository
{
    public RegionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
