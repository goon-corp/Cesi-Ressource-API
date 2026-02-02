using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Events.Models;
using Ressource_API.Features.Events.EventDtos;
using Ressource_API.Features.Events.Services;

namespace Ressource_API.Features.Events;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _service;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService service, ILogger<EventController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all events
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents(CancellationToken cancellationToken)
    {
        try
        {
            var events = await _service.GetAllEventsAsync(cancellationToken);
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all events");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving events");
        }
    }

    /// <summary>
    /// Get a evt by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Event>> GetEventById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var evt = await _service.GetEventByIdAsync(id, cancellationToken);

            if (evt == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(evt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving evt with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the evt");
        }
    }

    /// <summary>
    /// Create a new evt
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEvent = await _service.CreateEventAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetEventById),
                new { id = createdEvent.Id },
                createdEvent
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating evt");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the evt");
        }
    }

    /// <summary>
    /// Update an existing evt
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] UpdateEventDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedEvent = await _service.UpdateEventAsync(id, dto, cancellationToken);

            if (updatedEvent == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(updatedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating evt with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the evt");
        }
    }

    /// <summary>
    /// Delete a evt
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteEventAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting evt with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the evt");
        }
    }
}
