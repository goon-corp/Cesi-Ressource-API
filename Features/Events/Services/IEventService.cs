using System.Security.Claims;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;

namespace Ressource_API.Features.Events.Services;

public interface IEventService
{
    Task<Result<ReturnEventDto>> CreateEventAsync(CreateEventDto createEventDto, ClaimsPrincipal context, CancellationToken token = default);
    Task<Result<ReturnEventDto>> UpdateEventAsync(Guid eventId, UpdateEventDto updateEventDto, CancellationToken token = default);
    Task<Result<ReturnEventDto>> GetEventAsync(Guid ressourceId, CancellationToken token = default);
    Task<Result> DeleteEventAsync(Guid eventId, CancellationToken token = default);
}
