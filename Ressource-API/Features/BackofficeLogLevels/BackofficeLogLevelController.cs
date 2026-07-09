using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogLevels.BackofficeLogLevelDtos;
using Ressource_API.Features.BackofficeLogLevels.Services;

namespace Ressource_API.Features.BackofficeLogLevels;

[ApiController]
[Route("api/[controller]")]
public class BackofficeLogLevelController : ControllerBase
{
    private readonly IBackofficeLogLevelService _service;
    private readonly ILogger<BackofficeLogLevelController> _logger;

    public BackofficeLogLevelController(IBackofficeLogLevelService service, ILogger<BackofficeLogLevelController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all backofficeloglevels
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BackofficeLogLevel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BackofficeLogLevel>>> GetAllBackofficeLogLevels(CancellationToken cancellationToken)
    {
        try
        {
            var backofficeloglevels = await _service.GetAllBackofficeLogLevelsAsync(cancellationToken);
            return Ok(backofficeloglevels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all backofficeloglevels");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving backofficeloglevels");
        }
    }

    /// <summary>
    /// Get a backofficeloglevel by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BackofficeLogLevel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BackofficeLogLevel>> GetBackofficeLogLevelById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var backofficeloglevel = await _service.GetBackofficeLogLevelByIdAsync(id, cancellationToken);

            if (backofficeloglevel == null)
            {
                return NotFound($"BackofficeLogLevel with ID {id} not found");
            }

            return Ok(backofficeloglevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backofficeloglevel with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the backofficeloglevel");
        }
    }

    /// <summary>
    /// Create a new backofficeloglevel
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BackofficeLogLevel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeLogLevel>> CreateBackofficeLogLevel([FromBody] CreateBackofficeLogLevelDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBackofficeLogLevel = await _service.CreateBackofficeLogLevelAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBackofficeLogLevelById),
                new { id = createdBackofficeLogLevel.Id },
                createdBackofficeLogLevel
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backofficeloglevel");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the backofficeloglevel");
        }
    }

    /// <summary>
    /// Update an existing backofficeloglevel
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BackofficeLogLevel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeLogLevel>> UpdateBackofficeLogLevel(int id, [FromBody] UpdateBackofficeLogLevelDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBackofficeLogLevel = await _service.UpdateBackofficeLogLevelAsync(id, dto, cancellationToken);

            if (updatedBackofficeLogLevel == null)
            {
                return NotFound($"BackofficeLogLevel with ID {id} not found");
            }

            return Ok(updatedBackofficeLogLevel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating backofficeloglevel with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the backofficeloglevel");
        }
    }

    /// <summary>
    /// Delete a backofficeloglevel
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBackofficeLogLevel(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteBackofficeLogLevelAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"BackofficeLogLevel with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backofficeloglevel with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the backofficeloglevel");
        }
    }
}
