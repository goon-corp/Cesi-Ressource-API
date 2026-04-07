using Ressource_API.Features.Addresses.Models;
using Ressource_API.Features.Addresses.AddressDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Addresses.Factories;

public class AddressFactory : BaseFactory<Address>, IAddressFactory
{
    /// <summary>
    /// Creates a Address from a DTO
    /// </summary>
    public Address Create(CreateAddressDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Address CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Address
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateAddressDto dto)
        {
            // Create from DTO
            return new Address
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Address creation");
    }
}
