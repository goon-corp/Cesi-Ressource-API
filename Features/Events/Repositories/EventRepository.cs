using Ressource_API.Common.Data;
using Ressource_API.Features.Events.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Events.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }
}
