using Ressource_API.Features.Notifications.Models;
using Ressource_API.Features.Notifications.NotificationDtos;

namespace Ressource_API.Features.Notifications.Services;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetAllNotificationsAsync(CancellationToken cancellationToken = default);
    Task<Notification?> GetNotificationByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Notification> CreateNotificationAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default);
    Task<Notification?> UpdateNotificationAsync(int id, UpdateNotificationDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteNotificationAsync(int id, CancellationToken cancellationToken = default);
}
