using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.Notifications.NotificationDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Notifications.Factories;

public class NotificationFactory : BaseFactory<Notification>, INotificationFactory
{
    /// <summary>
    /// Creates a Notification from a DTO
    /// </summary>
    public Notification Create(CreateNotificationDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Notification CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Notification
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateNotificationDto dto)
        {
            // Create from DTO
            return new Notification
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Notification creation");
    }
}
