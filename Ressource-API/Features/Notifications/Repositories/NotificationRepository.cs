using Ressource_API.Common.Data;
using Ressource_API.Features.Notifications.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Notifications.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context)
    {
    }
}
