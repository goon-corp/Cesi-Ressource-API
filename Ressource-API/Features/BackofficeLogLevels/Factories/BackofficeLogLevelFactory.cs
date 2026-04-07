using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogLevels.BackofficeLogLevelDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.BackofficeLogLevels.Factories;

public class BackofficeLogLevelFactory : BaseFactory<BackofficeLogLevel>, IBackofficeLogLevelFactory
{
    /// <summary>
    /// Creates a BackofficeLogLevel from a DTO
    /// </summary>
    public BackofficeLogLevel Create(CreateBackofficeLogLevelDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override BackofficeLogLevel CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new BackofficeLogLevel
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateBackofficeLogLevelDto dto)
        {
            // Create from DTO
            return new BackofficeLogLevel
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for BackofficeLogLevel creation");
    }
}
