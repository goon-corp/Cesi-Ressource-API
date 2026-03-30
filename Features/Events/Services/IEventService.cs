using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Query;

namespace Ressource_API.Features.Events.Services;

public interface IEventService
{
    Task<Result<ReturnEventDto>> CreateEventAsync(CreateEventDto createEventDto, ClaimsPrincipal context, CancellationToken token = default);
    Task<Result<ReturnEventDto>> UpdateEventAsync(Guid eventId, UpdateEventDto updateEventDto, CancellationToken token = default);
    Task<Result<ReturnEventDto>> GetEventAsync(Guid ressourceId, CancellationToken token = default);
    Task<Result> DeleteEventAsync(Guid eventId, CancellationToken token = default);

    Task<Result<PaginatedList<EventMemberDto>>> GetEventMembersAsync(Guid eventId, EventMemberQuery query, CancellationToken token = default);
    Task<Result> AddMemberAsync(Guid eventId, Guid userId, ClaimsPrincipal context, CancellationToken token = default);
    Task<Result> RemoveMemberAsync(Guid eventId, Guid userId, ClaimsPrincipal context, CancellationToken token = default);
    Task<Result> JoinEventAsync(Guid eventId, ClaimsPrincipal context, CancellationToken token = default);
    Task<Result> LeaveEventAsync(Guid eventId, ClaimsPrincipal context, CancellationToken token = default);
}
