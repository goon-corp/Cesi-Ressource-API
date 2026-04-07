using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RessourceStatuses.Factories;

public class RessourceStatusFactory : BaseFactory<RessourceStatus>, IRessourceStatusFactory
{
    /// <summary>
    /// Creates a RessourceStatus from a DTO
    /// </summary>
    public RessourceStatus Create(CreateRessourceStatusDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RessourceStatus CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RessourceStatus
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRessourceStatusDto dto)
        {
            // Create from DTO
            return new RessourceStatus
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RessourceStatus creation");
    }
}
