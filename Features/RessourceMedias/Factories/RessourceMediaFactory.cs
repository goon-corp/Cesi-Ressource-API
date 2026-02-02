using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.RessourceMediaDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceMedias.Factories;

public class RessourceMediaFactory : BaseFactory<RessourceMedia>, IRessourceMediaFactory
{
    /// <summary>
    /// Creates a RessourceMedia from a DTO
    /// </summary>
    public RessourceMedia Create(CreateRessourceMediaDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RessourceMedia CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RessourceMedia
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceMediaDto dto)
        {
            // Create from DTO
            return new RessourceMedia
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RessourceMedia creation");
    }
}
