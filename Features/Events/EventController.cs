using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Query;
using Ressource_API.Features.Events.Services;

namespace Ressource_API.Features.Events;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> CreateEvent([FromForm] CreateEventDto createEventDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _eventService.CreateEventAsync(createEventDto, User);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        _logger.LogInformation("Created event and linked ressource");
        return Ok(result.Data);
    }

    [HttpPut("{eventId}")]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> UpdateEvent([FromRoute] Guid eventId, [FromBody] UpdateEventDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _eventService.UpdateEventAsync(eventId, dto);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        _logger.LogInformation("Updated event and its linked ressource");
        return Ok(result.Data);
    }

    [HttpGet("{ressourceId}")]
    public async Task<ActionResult<ReturnEventDto>> GetEvent([FromRoute] Guid ressourceId)
    {
        var result = await _eventService.GetEventAsync(ressourceId);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        _logger.LogInformation("Fetched event and its linked ressource");
        return Ok(result.Data);
    }

    [HttpDelete("{eventId}")]
    [Authorize]
    public async Task<ActionResult> DeleteEvent([FromRoute] Guid eventId)
    {
        var result = await _eventService.DeleteEventAsync(eventId);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        _logger.LogInformation("Deleted event and its linked ressource");
        return NoContent();
    }

    [HttpGet("{eventId}/members")]
    public async Task<ActionResult<PaginatedList<EventMemberDto>>> GetEventMembers(
        [FromRoute] Guid eventId,
        [FromQuery] EventMemberQuery query)
    {
        var result = await _eventService.GetEventMembersAsync(eventId, query);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Data);
    }

    [HttpPost("{eventId}/members/{userId}")]
    [Authorize]
    public async Task<ActionResult> AddMember([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var result = await _eventService.AddMemberAsync(eventId, userId, User);

        if (!result.IsSuccess)
        {
            if (result.Error == "Event not found" || result.Error == "User not found")
                return NotFound(result.Error);

            if (result.Error == "You are not authorized to perform this action")
                return Forbid();

            return BadRequest(result.Error);
        }

        _logger.LogInformation("Added member {UserId} to event {EventId}", userId, eventId);
        return NoContent();
    }

    [HttpDelete("{eventId}/members/{userId}")]
    [Authorize]
    public async Task<ActionResult> RemoveMember([FromRoute] Guid eventId, [FromRoute] Guid userId)
    {
        var result = await _eventService.RemoveMemberAsync(eventId, userId, User);

        if (!result.IsSuccess)
        {
            if (result.Error == "Event not found")
                return NotFound(result.Error);

            if (result.Error == "You are not authorized to perform this action")
                return Forbid();

            return BadRequest(result.Error);
        }

        _logger.LogInformation("Removed member {UserId} from event {EventId}", userId, eventId);
        return NoContent();
    }

    [HttpPost("{eventId}/members/join")]
    [Authorize]
    public async Task<ActionResult> JoinEvent([FromRoute] Guid eventId)
    {
        var result = await _eventService.JoinEventAsync(eventId, User);

        if (!result.IsSuccess)
        {
            if (result.Error == "Event not found")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        _logger.LogInformation("User joined event {EventId}", eventId);
        return NoContent();
    }

    [HttpDelete("{eventId}/members/leave")]
    [Authorize]
    public async Task<ActionResult> LeaveEvent([FromRoute] Guid eventId)
    {
        var result = await _eventService.LeaveEventAsync(eventId, User);

        if (!result.IsSuccess)
        {
            if (result.Error == "Event not found")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        _logger.LogInformation("User left event {EventId}", eventId);
        return NoContent();
    }
}
