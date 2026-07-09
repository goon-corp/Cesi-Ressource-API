using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Cities.CityDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Cities.Factories;

public class CityFactory : BaseFactory<City>, ICityFactory
{
    /// <summary>
    /// Creates a City from a DTO
    /// </summary>
    public City Create(CreateCityDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override City CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new City
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateCityDto dto)
        {
            // Create from DTO
            return new City
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for City creation");
    }
}
