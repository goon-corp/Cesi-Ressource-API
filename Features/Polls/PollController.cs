using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.PollDtos;
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
    /// Get all polls
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Poll>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Poll>>> GetAllPolls(CancellationToken cancellationToken)
    {
        try
        {
            var polls = await _service.GetAllPollsAsync(cancellationToken);
            return Ok(polls);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all polls");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving polls");
        }
    }

    /// <summary>
    /// Get a poll by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Poll), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Poll>> GetPollById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var poll = await _service.GetPollByIdAsync(id, cancellationToken);

            if (poll == null)
            {
                return NotFound($"Poll with ID {id} not found");
            }

            return Ok(poll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving poll with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the poll");
        }
    }

    /// <summary>
    /// Create a new poll
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Poll), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Poll>> CreatePoll([FromBody] CreatePollDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPoll = await _service.CreatePollAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetPollById),
                new { id = createdPoll.Id },
                createdPoll
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating poll");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the poll");
        }
    }

    /// <summary>
    /// Update an existing poll
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Poll), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Poll>> UpdatePoll(int id, [FromBody] UpdatePollDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPoll = await _service.UpdatePollAsync(id, dto, cancellationToken);

            if (updatedPoll == null)
            {
                return NotFound($"Poll with ID {id} not found");
            }

            return Ok(updatedPoll);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating poll with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the poll");
        }
    }

    /// <summary>
    /// Delete a poll
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePoll(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeletePollAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Poll with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting poll with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the poll");
        }
    }
}
