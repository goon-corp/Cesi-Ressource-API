using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Services;

public interface IRessourceConfidentialityTypeService
{
    Task<Result<IEnumerable<RessourceConfidentialityTypeInfoDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
