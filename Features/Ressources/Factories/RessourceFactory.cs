using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Ressources.Factories;

public class RessourceFactory : BaseFactory<Ressource>, IRessourceFactory
{
    /// <summary>
    /// Creates a Ressource from a DTO
    /// </summary>
    public Ressource Create(CreateRessourceDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Ressource CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Ressource
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceDto dto)
        {
            // Create from DTO
            return new Ressource
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Ressource creation");
    }
}
