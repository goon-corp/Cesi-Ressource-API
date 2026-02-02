using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Factories;

public class RessourceConfidentialityTypeFactory : BaseFactory<RessourceConfidentialityType>, IRessourceConfidentialityTypeFactory
{
    /// <summary>
    /// Creates a RessourceConfidentialityType from a DTO
    /// </summary>
    public RessourceConfidentialityType Create(CreateRessourceConfidentialityTypeDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RessourceConfidentialityType CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RessourceConfidentialityType
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceConfidentialityTypeDto dto)
        {
            // Create from DTO
            return new RessourceConfidentialityType
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RessourceConfidentialityType creation");
    }
}
