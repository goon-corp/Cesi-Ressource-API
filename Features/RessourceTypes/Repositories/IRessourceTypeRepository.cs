using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Repositories;

public interface IRessourceTypeRepository : IBaseRepository<RessourceType>
{
    Task<IEnumerable<ReturnRessourceTypeDTO>> ListAllAsync(CancellationToken cancellationToken = default);
}
