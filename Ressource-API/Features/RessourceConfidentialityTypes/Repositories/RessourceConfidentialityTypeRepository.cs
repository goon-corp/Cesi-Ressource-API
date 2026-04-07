using Ressource_API.Common.Data;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Repositories;

public class RessourceConfidentialityTypeRepository : BaseRepository<RessourceConfidentialityType>, IRessourceConfidentialityTypeRepository
{
    public RessourceConfidentialityTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
