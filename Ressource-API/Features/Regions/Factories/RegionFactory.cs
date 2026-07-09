using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Regions.RegionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Regions.Factories;

public class RegionFactory : BaseFactory<Region>, IRegionFactory
{
    /// <summary>
    /// Creates a Region from a DTO
    /// </summary>
    public Region Create(CreateRegionDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Region CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Region
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRegionDto dto)
        {
            // Create from DTO
            return new Region
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Region creation");
    }
}
