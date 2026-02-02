using Ressource_API.Common.Data;
using Ressource_API.Features.Cities.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Cities.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
