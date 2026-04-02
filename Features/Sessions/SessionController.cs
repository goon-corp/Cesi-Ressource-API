using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.SessionMessages.Dtos;
using Ressource_API.Features.SessionMessages.Services;
using Ressource_API.Features.Sessions.Dtos;
using Ressource_API.Features.Sessions.Services;

namespace Ressource_API.Features.Sessions;

[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _service;
    private readonly ISessionMessageService _messageService;
    private readonly ILogger<SessionController> _logger;

    public SessionController(ISessionService service, ISessionMessageService messageService, ILogger<SessionController> logger)
    {
        _service = service;
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// Get all sessions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReturnSessionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSessions(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllSessionsAsync(cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a session by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReturnSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetSessionByIdAsync(id, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: NotFound);
    }

    /// <summary>
    /// Get all messages for a session (poll for updates)
    /// </summary>
    [HttpGet("{id:guid}/messages")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<ReturnSessionMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionMessages([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var sessionResult = await _service.GetSessionByIdAsync(id, cancellationToken);
        if (!sessionResult.IsSuccess) return NotFound(sessionResult.Error);

        var result = await _messageService.GetBySessionIdAsync(id, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Create a new session
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ReturnSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.CreateSessionAsync(dto, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: created => CreatedAtAction(nameof(GetSessionById), new { id = created.Id }, created),
            onFailure: BadRequest);
    }

    /// <summary>
    /// Update session status
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ReturnSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSession([FromRoute] Guid id, [FromBody] UpdateSessionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.UpdateSessionAsync(id, dto, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: NotFound);
    }

    /// <summary>
    /// Delete a session
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteSessionAsync(id, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: NoContent,
            onFailure: NotFound);
    }
}
