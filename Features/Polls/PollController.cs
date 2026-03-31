using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Query;
using Ressource_API.Features.Polls.Services;

namespace Ressource_API.Features.Polls;

[ApiController]
[Route("api/[controller]")]
public class PollController : ControllerBase
{
    private readonly IPollService _service;
    private readonly ILogger<PollController> _logger;

    public PollController(IPollService service, ILogger<PollController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all polls (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<PollInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<PollInfoDto>>> GetPaginatedPolls(
        [FromQuery] PollQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedPollsAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a poll by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PollInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollInfoDto>> GetPollById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPollByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new poll
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PollInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollInfoDto>> CreatePoll(
        [FromBody] CreatePollDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreatePollAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetPollById),
                new { id = data.Id },
                data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Update a poll
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PollInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollInfoDto>> UpdatePoll(
        Guid id,
        [FromBody] UpdatePollDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdatePollAsync(id, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete a poll
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePoll(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeletePollAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}