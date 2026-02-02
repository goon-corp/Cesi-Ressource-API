using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.EmailLogs.Factories;

public class EmailLogFactory : BaseFactory<EmailLog>, IEmailLogFactory
{
    /// <summary>
    /// Creates a EmailLog from a DTO
    /// </summary>
    public EmailLog Create(CreateEmailLogDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override EmailLog CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new EmailLog
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateEmailLogDto dto)
        {
            // Create from DTO
            return new EmailLog
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for EmailLog creation");
    }
}
