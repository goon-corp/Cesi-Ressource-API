using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Services;

namespace Ressource_API.Features.Events;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> CreateEvent([FromForm] CreateEventDto createEventDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var returnedEvent = await _eventService.CreateEventAsync(createEventDto, User);
            _logger.LogInformation($"Created event and linked ressource");
            return Ok(returnedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating an event");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating an event.");
        }
    }
    
 
    [HttpPut("{eventId}")]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> UpdateEvent( [FromRoute] Guid eventId, [FromBody] UpdateEventDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var returnedEvent = await _eventService.UpdateEventAsync(eventId,dto);
            _logger.LogInformation($"Updated an event and it's linked Ressource");
            return Ok(returnedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating an event");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while updating an event.");
        }
    }
    
    [HttpGet("{ressourceId}")]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> GetEvent( [FromRoute] Guid ressourceId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var returnedEvent = await _eventService.GetEventAsync(ressourceId);
            _logger.LogInformation($"Fetched an event and it's linked Ressource");
            return Ok(returnedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching an event");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while fetching an event.");
        }
    }
    
    [HttpDelete("{eventId}")]
    [Authorize]
    public async Task<ActionResult<ReturnEventDto>> DeleteEvent( [FromRoute] Guid eventId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var returnedEvent = await _eventService.DeleteEventAsync(eventId);
            _logger.LogInformation($"Deleted an event and it's linked Ressource");
            return Ok(returnedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting an event");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while fetching an event.");
        }
    }
}