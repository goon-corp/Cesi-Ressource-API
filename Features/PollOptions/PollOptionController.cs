using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.PollOptionDtos;
using Ressource_API.Features.PollOptions.Services;

namespace Ressource_API.Features.PollOptions;

[ApiController]
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
    /// Get all polloptions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PollOption>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PollOption>>> GetAllPollOptions(CancellationToken cancellationToken)
    {
        try
        {
            var polloptions = await _service.GetAllPollOptionsAsync(cancellationToken);
            return Ok(polloptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all polloptions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving polloptions");
        }
    }

    /// <summary>
    /// Get a polloption by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PollOption), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PollOption>> GetPollOptionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var polloption = await _service.GetPollOptionByIdAsync(id, cancellationToken);

            if (polloption == null)
            {
                return NotFound($"PollOption with ID {id} not found");
            }

            return Ok(polloption);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving polloption with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the polloption");
        }
    }

    /// <summary>
    /// Create a new polloption
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PollOption), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollOption>> CreatePollOption([FromBody] CreatePollOptionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPollOption = await _service.CreatePollOptionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetPollOptionById),
                new { id = createdPollOption.Id },
                createdPollOption
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating polloption");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the polloption");
        }
    }

    /// <summary>
    /// Update an existing polloption
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PollOption), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PollOption>> UpdatePollOption(int id, [FromBody] UpdatePollOptionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPollOption = await _service.UpdatePollOptionAsync(id, dto, cancellationToken);

            if (updatedPollOption == null)
            {
                return NotFound($"PollOption with ID {id} not found");
            }

            return Ok(updatedPollOption);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating polloption with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the polloption");
        }
    }

    /// <summary>
    /// Delete a polloption
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePollOption(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeletePollOptionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"PollOption with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting polloption with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the polloption");
        }
    }
}
