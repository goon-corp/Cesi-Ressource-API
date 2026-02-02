using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Events.Factories;

public class EventFactory : BaseFactory<Event>, IEventFactory
{
    /// <summary>
    /// Creates a Event from a DTO
    /// </summary>
    public Event Create(CreateEventDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Event CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Event
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateEventDto dto)
        {
            // Create from DTO
            return new Event
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Event creation");
    }
}
