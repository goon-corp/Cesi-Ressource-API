using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;

namespace Ressource_API.Features.Events.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default);
    Task<Event?> GetEventByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Event> CreateEventAsync(CreateEventDto dto, CancellationToken cancellationToken = default);
    Task<Event?> UpdateEventAsync(int id, UpdateEventDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteEventAsync(int id, CancellationToken cancellationToken = default);
}
