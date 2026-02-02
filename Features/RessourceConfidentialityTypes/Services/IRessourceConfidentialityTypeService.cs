using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Services;

public interface IRessourceConfidentialityTypeService
{
    Task<IEnumerable<RessourceConfidentialityType>> GetAllRessourceConfidentialityTypesAsync(CancellationToken cancellationToken = default);
    Task<RessourceConfidentialityType?> GetRessourceConfidentialityTypeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RessourceConfidentialityType> CreateRessourceConfidentialityTypeAsync(CreateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken = default);
    Task<RessourceConfidentialityType?> UpdateRessourceConfidentialityTypeAsync(int id, UpdateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceConfidentialityTypeAsync(int id, CancellationToken cancellationToken = default);
}
