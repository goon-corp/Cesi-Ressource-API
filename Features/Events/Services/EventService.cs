using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Extensions;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.Repositories;
using Ressource_API.Features.Ressources.Extensions;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Events.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IRessourceRepository _ressourceRepository;
    private readonly IRessourceService _ressourceService;

    public EventService(
        IEventRepository eventRepository, IRessourceService ressourceService, IRessourceRepository ressourceRepository)
    {
        _eventRepository = eventRepository;
        _ressourceRepository = ressourceRepository;
        _ressourceService = ressourceService;
    }

    public async Task<Result<ReturnEventDto>> GetEventAsync(Guid ressourceId, CancellationToken token = default)
    {
        var existing = await _eventRepository.FirstOrDefaultAsyncAsNoTracking(e => e.RessourceId == ressourceId, token);
        if (existing is null) throw new KeyNotFoundException("Cannot found Event");

        return new Result<ReturnEventDto>(existing.ToReturnEventDto(existing.Ressource.ToReturnDto()), true, null);
    }
    
    public async Task<Result<ReturnEventDto>> CreateEventAsync(CreateEventDto createEventDto, ClaimsPrincipal context, CancellationToken token = default)
    {

        var createdRessource = await _ressourceService.CreateRessourceAsync(createEventDto.RessourceInfos,context);

        var eventToAdd = createEventDto.ToEvent(createdRessource.Id);

        Event createdEvent = await _eventRepository.AddAsync(eventToAdd);

        return new Result<ReturnEventDto>(createdEvent.ToReturnEventDto(createdRessource), true, null);
    }

    public async Task<Result<ReturnEventDto>> UpdateEventAsync(Guid eventId, UpdateEventDto updateEventDto, CancellationToken token = default)
    {
        var updatedRessource = await _ressourceService.UpdateRessourceAsync(updateEventDto.RessourceId,updateEventDto.Ressource, token);

        var existingEvent = await _eventRepository.FindAsync(eventId, token);

        if (existingEvent is null) throw new KeyNotFoundException("Cannot found Event");

        existingEvent.DateStart = updateEventDto.DateStart;
        existingEvent.DateEnd = updateEventDto.DateEnd;
        existingEvent.EventLink = updateEventDto.EventLink;
        existingEvent.Location = updateEventDto.Location;
        existingEvent.IsVirtual = updateEventDto.IsVirtual;
        existingEvent.RessourceId = updateEventDto.RessourceId;
        
        await _eventRepository.UpdateAsync(existingEvent, token);
        return new Result<ReturnEventDto>(existingEvent.ToReturnEventDto(updatedRessource), true, null);
    }

    public async Task<Result> DeleteEventAsync(Guid eventId, CancellationToken token = default)
    {
        var eventToDelete = await _eventRepository.FindAsync(eventId);
        
        if(eventToDelete is null) throw new KeyNotFoundException("Cannot found Event to delete");
        
        var ressourceToDelete = await _ressourceRepository.FirstOrDefaultAsync(r => r.Id == eventToDelete.RessourceId );
        
        if(ressourceToDelete is null) throw new KeyNotFoundException("Cannot found Ressource to delete");

        await _eventRepository.DeleteAsync(eventToDelete);
        await _ressourceRepository.DeleteAsync(ressourceToDelete);
        
        return Result.Success();
    }
}
