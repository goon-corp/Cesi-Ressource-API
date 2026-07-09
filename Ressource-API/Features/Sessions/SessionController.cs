using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.SessionDtos;
using Ressource_API.Features.Sessions.Services;

namespace Ressource_API.Features.Sessions;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _service;
    private readonly ILogger<SessionController> _logger;

    public SessionController(ISessionService service, ILogger<SessionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all sessions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Session>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Session>>> GetAllSessions(CancellationToken cancellationToken)
    {
        try
        {
            var sessions = await _service.GetAllSessionsAsync(cancellationToken);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all sessions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving sessions");
        }
    }

    /// <summary>
    /// Get a session by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Session>> GetSessionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var session = await _service.GetSessionByIdAsync(id, cancellationToken);

            if (session == null)
            {
                return NotFound($"Session with ID {id} not found");
            }

            return Ok(session);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving session with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the session");
        }
    }

    /// <summary>
    /// Create a new session
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Session), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Session>> CreateSession([FromBody] CreateSessionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSession = await _service.CreateSessionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetSessionById),
                new { id = createdSession.Id },
                createdSession
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating session");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the session");
        }
    }

    /// <summary>
    /// Update an existing session
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Session>> UpdateSession(int id, [FromBody] UpdateSessionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSession = await _service.UpdateSessionAsync(id, dto, cancellationToken);

            if (updatedSession == null)
            {
                return NotFound($"Session with ID {id} not found");
            }

            return Ok(updatedSession);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating session with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the session");
        }
    }

    /// <summary>
    /// Delete a session
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSession(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteSessionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Session with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting session with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the session");
        }
    }
}
