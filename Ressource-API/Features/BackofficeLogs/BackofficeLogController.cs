using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeLogs.BackofficeLogDtos;
using Ressource_API.Features.BackofficeLogs.Services;

namespace Ressource_API.Features.BackofficeLogs;

[ApiController]
[Route("api/[controller]")]
public class BackofficeLogController : ControllerBase
{
    private readonly IBackofficeLogService _service;
    private readonly ILogger<BackofficeLogController> _logger;

    public BackofficeLogController(IBackofficeLogService service, ILogger<BackofficeLogController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all backofficelogs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BackofficeLog>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BackofficeLog>>> GetAllBackofficeLogs(CancellationToken cancellationToken)
    {
        try
        {
            var backofficelogs = await _service.GetAllBackofficeLogsAsync(cancellationToken);
            return Ok(backofficelogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all backofficelogs");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving backofficelogs");
        }
    }

    /// <summary>
    /// Get a backofficelog by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BackofficeLog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BackofficeLog>> GetBackofficeLogById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var backofficelog = await _service.GetBackofficeLogByIdAsync(id, cancellationToken);

            if (backofficelog == null)
            {
                return NotFound($"BackofficeLog with ID {id} not found");
            }

            return Ok(backofficelog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backofficelog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the backofficelog");
        }
    }

    /// <summary>
    /// Create a new backofficelog
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BackofficeLog), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeLog>> CreateBackofficeLog([FromBody] CreateBackofficeLogDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBackofficeLog = await _service.CreateBackofficeLogAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBackofficeLogById),
                new { id = createdBackofficeLog.Id },
                createdBackofficeLog
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backofficelog");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the backofficelog");
        }
    }

    /// <summary>
    /// Update an existing backofficelog
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BackofficeLog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeLog>> UpdateBackofficeLog(int id, [FromBody] UpdateBackofficeLogDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBackofficeLog = await _service.UpdateBackofficeLogAsync(id, dto, cancellationToken);

            if (updatedBackofficeLog == null)
            {
                return NotFound($"BackofficeLog with ID {id} not found");
            }

            return Ok(updatedBackofficeLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating backofficelog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the backofficelog");
        }
    }

    /// <summary>
    /// Delete a backofficelog
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBackofficeLog(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteBackofficeLogAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"BackofficeLog with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backofficelog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the backofficelog");
        }
    }
}
