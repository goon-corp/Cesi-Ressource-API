using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Repositories;
using Ressource_API.Features.Events.Factories;

namespace Ressource_API.Features.Events.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    private readonly IEventFactory _factory;

    public EventService(
        IEventRepository repository,
        IEventFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Event?> GetEventByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Event> CreateEventAsync(CreateEventDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var evt = _factory.Create(dto);
        
        return await _repository.AddAsync(evt, cancellationToken);
    }

    public async Task<Event?> UpdateEventAsync(int id, UpdateEventDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteEventAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
