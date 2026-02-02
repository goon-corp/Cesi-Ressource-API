using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceProgressions.Factories;

public class RessourceProgressionFactory : BaseFactory<RessourceProgression>, IRessourceProgressionFactory
{
    /// <summary>
    /// Creates a RessourceProgression from a DTO
    /// </summary>
    public RessourceProgression Create(CreateRessourceProgressionDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RessourceProgression CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RessourceProgression
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceProgressionDto dto)
        {
            // Create from DTO
            return new RessourceProgression
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RessourceProgression creation");
    }
}
