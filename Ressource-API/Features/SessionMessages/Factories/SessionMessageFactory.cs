using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.SessionMessageDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.SessionMessages.Factories;

public class SessionMessageFactory : BaseFactory<SessionMessage>, ISessionMessageFactory
{
    /// <summary>
    /// Creates a SessionMessage from a DTO
    /// </summary>
    public SessionMessage Create(CreateSessionMessageDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override SessionMessage CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new SessionMessage
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateSessionMessageDto dto)
        {
            // Create from DTO
            return new SessionMessage
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for SessionMessage creation");
    }
}
