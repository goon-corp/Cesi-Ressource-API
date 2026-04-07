using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Services;

public interface IRessourceTypeService
{
    Task<IEnumerable<ReturnRessourceTypeDTO>> GetAllRessourceTypesAsync(CancellationToken cancellationToken = default);
}
