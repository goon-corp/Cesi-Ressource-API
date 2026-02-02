using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.BackofficeOperationTypes.BackofficeOperationTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeOperationTypes.Factories;

public class BackofficeOperationTypeFactory : BaseFactory<BackofficeOperationType>, IBackofficeOperationTypeFactory
{
    /// <summary>
    /// Creates a BackofficeOperationType from a DTO
    /// </summary>
    public BackofficeOperationType Create(CreateBackofficeOperationTypeDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override BackofficeOperationType CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new BackofficeOperationType
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateBackofficeOperationTypeDto dto)
        {
            // Create from DTO
            return new BackofficeOperationType
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for BackofficeOperationType creation");
    }
}
