using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.SessionMessages.Dtos;
using Ressource_API.Features.SessionMessages.Services;

namespace Ressource_API.Features.SessionMessages;

[ApiController]
[Route("api/session-messages")]
public class SessionMessageController : ControllerBase
{
    private readonly ISessionMessageService _service;

    public SessionMessageController(ISessionMessageService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get a session message by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ReturnSessionMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionMessageById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetSessionMessageByIdAsync(id, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: NotFound);
    }

    /// <summary>
    /// Send a message in a session
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ReturnSessionMessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSessionMessage([FromBody] CreateSessionMessageDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.CreateSessionMessageAsync(dto, User, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: created => CreatedAtAction(nameof(GetSessionMessageById), new { id = created.Id }, created),
            onFailure: BadRequest);
    }

    /// <summary>
    /// Edit a message
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ReturnSessionMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSessionMessage([FromRoute] Guid id, [FromBody] UpdateSessionMessageDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.UpdateSessionMessageAsync(id, dto, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: NotFound);
    }

    /// <summary>
    /// Delete a message
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSessionMessage([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteSessionMessageAsync(id, cancellationToken);
        return result.Match<IActionResult>(
            onSuccess: NoContent,
            onFailure: NotFound);
    }
}
