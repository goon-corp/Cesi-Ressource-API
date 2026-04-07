using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.Notifications.NotificationDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Notifications.Factories;

public interface INotificationFactory : IBaseFactory<Notification>
{
    Notification Create(CreateNotificationDto dto);
}
