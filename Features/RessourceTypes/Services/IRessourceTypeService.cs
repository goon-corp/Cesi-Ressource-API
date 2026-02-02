using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;

namespace Ressource_API.Features.RessourceTypes.Services;

public interface IRessourceTypeService
{
    Task<IEnumerable<RessourceType>> GetAllRessourceTypesAsync(CancellationToken cancellationToken = default);
    Task<RessourceType?> GetRessourceTypeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RessourceType> CreateRessourceTypeAsync(CreateRessourceTypeDto dto, CancellationToken cancellationToken = default);
    Task<RessourceType?> UpdateRessourceTypeAsync(int id, UpdateRessourceTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceTypeAsync(int id, CancellationToken cancellationToken = default);
}
