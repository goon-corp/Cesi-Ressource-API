using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceTypes.Factories;

public class RessourceTypeFactory : BaseFactory<RessourceType>, IRessourceTypeFactory
{
    /// <summary>
    /// Creates a RessourceType from a DTO
    /// </summary>
    public RessourceType Create(CreateRessourceTypeDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RessourceType CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RessourceType
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceTypeDto dto)
        {
            // Create from DTO
            return new RessourceType
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RessourceType creation");
    }
}
