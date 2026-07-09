using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Query;
using Ressource_API.Features.PollOptions.Services;

namespace Ressource_API.Features.PollOptions;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PollOptionController : ControllerBase
{
    private readonly IPollOptionService _service;
    private readonly ILogger<PollOptionController> _logger;

    public PollOptionController(IPollOptionService service, ILogger<PollOptionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all poll options (paginated)
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<PollOptionInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<PollOptionInfoDto>>> GetPaginatedPollOptions(
        [FromQuery] PollOptionQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedPollOptionsAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a poll option by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PollOptionInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollOptionInfoDto>> GetPollOptionById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPollOptionByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new poll option
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PollOptionInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollOptionInfoDto>> CreatePollOption(
        [FromBody] CreatePollOptionDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreatePollOptionAsync(dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetPollOptionById),
                new { id = data.Id },
                data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Vote for a poll option
    /// </summary>
    [HttpPost("{id:guid}/vote")]
    [ProducesResponseType(typeof(PollOptionInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollOptionInfoDto>> VotePollOption(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.VotePollOptionAsync(id, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => BadRequest(error));
    }

    /// <summary>
    /// Soft delete a poll option
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePollOption(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeletePollOptionAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}
