using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeLogs.BackofficeLogDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeLogs.Factories;

public class BackofficeLogFactory : BaseFactory<BackofficeLog>, IBackofficeLogFactory
{
    /// <summary>
    /// Creates a BackofficeLog from a DTO
    /// </summary>
    public BackofficeLog Create(CreateBackofficeLogDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override BackofficeLog CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new BackofficeLog
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateBackofficeLogDto dto)
        {
            // Create from DTO
            return new BackofficeLog
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for BackofficeLog creation");
    }
}
