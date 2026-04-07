using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.SessionMessageDtos;
using Ressource_API.Features.SessionMessages.Services;

namespace Ressource_API.Features.SessionMessages;

[ApiController]
[Route("api/[controller]")]
public class SessionMessageController : ControllerBase
{
    private readonly ISessionMessageService _service;
    private readonly ILogger<SessionMessageController> _logger;

    public SessionMessageController(ISessionMessageService service, ILogger<SessionMessageController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all sessionmessages
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SessionMessage>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SessionMessage>>> GetAllSessionMessages(CancellationToken cancellationToken)
    {
        try
        {
            var sessionmessages = await _service.GetAllSessionMessagesAsync(cancellationToken);
            return Ok(sessionmessages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all sessionmessages");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving sessionmessages");
        }
    }

    /// <summary>
    /// Get a sessionmessage by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SessionMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessionMessage>> GetSessionMessageById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var sessionmessage = await _service.GetSessionMessageByIdAsync(id, cancellationToken);

            if (sessionmessage == null)
            {
                return NotFound($"SessionMessage with ID {id} not found");
            }

            return Ok(sessionmessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sessionmessage with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the sessionmessage");
        }
    }

    /// <summary>
    /// Create a new sessionmessage
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SessionMessage), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionMessage>> CreateSessionMessage([FromBody] CreateSessionMessageDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSessionMessage = await _service.CreateSessionMessageAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetSessionMessageById),
                new { id = createdSessionMessage.Id },
                createdSessionMessage
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sessionmessage");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the sessionmessage");
        }
    }

    /// <summary>
    /// Update an existing sessionmessage
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SessionMessage), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SessionMessage>> UpdateSessionMessage(int id, [FromBody] UpdateSessionMessageDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSessionMessage = await _service.UpdateSessionMessageAsync(id, dto, cancellationToken);

            if (updatedSessionMessage == null)
            {
                return NotFound($"SessionMessage with ID {id} not found");
            }

            return Ok(updatedSessionMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sessionmessage with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the sessionmessage");
        }
    }

    /// <summary>
    /// Delete a sessionmessage
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSessionMessage(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteSessionMessageAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"SessionMessage with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting sessionmessage with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the sessionmessage");
        }
    }
}
