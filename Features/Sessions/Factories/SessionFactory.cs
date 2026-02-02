using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.SessionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Sessions.Factories;

public class SessionFactory : BaseFactory<Session>, ISessionFactory
{
    /// <summary>
    /// Creates a Session from a DTO
    /// </summary>
    public Session Create(CreateSessionDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Session CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Session
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateSessionDto dto)
        {
            // Create from DTO
            return new Session
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Session creation");
    }
}
