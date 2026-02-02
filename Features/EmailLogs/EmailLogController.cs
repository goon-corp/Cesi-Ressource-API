using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;
using Ressource_API.Features.EmailLogs.Services;

namespace Ressource_API.Features.EmailLogs;

[ApiController]
[Route("api/[controller]")]
public class EmailLogController : ControllerBase
{
    private readonly IEmailLogService _service;
    private readonly ILogger<EmailLogController> _logger;

    public EmailLogController(IEmailLogService service, ILogger<EmailLogController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all emaillogs
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmailLog>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmailLog>>> GetAllEmailLogs(CancellationToken cancellationToken)
    {
        try
        {
            var emaillogs = await _service.GetAllEmailLogsAsync(cancellationToken);
            return Ok(emaillogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all emaillogs");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving emaillogs");
        }
    }

    /// <summary>
    /// Get a emaillog by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmailLog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailLog>> GetEmailLogById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var emaillog = await _service.GetEmailLogByIdAsync(id, cancellationToken);

            if (emaillog == null)
            {
                return NotFound($"EmailLog with ID {id} not found");
            }

            return Ok(emaillog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving emaillog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the emaillog");
        }
    }

    /// <summary>
    /// Create a new emaillog
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmailLog), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmailLog>> CreateEmailLog([FromBody] CreateEmailLogDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEmailLog = await _service.CreateEmailLogAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetEmailLogById),
                new { id = createdEmailLog.Id },
                createdEmailLog
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating emaillog");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the emaillog");
        }
    }

    /// <summary>
    /// Update an existing emaillog
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmailLog), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmailLog>> UpdateEmailLog(int id, [FromBody] UpdateEmailLogDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedEmailLog = await _service.UpdateEmailLogAsync(id, dto, cancellationToken);

            if (updatedEmailLog == null)
            {
                return NotFound($"EmailLog with ID {id} not found");
            }

            return Ok(updatedEmailLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating emaillog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the emaillog");
        }
    }

    /// <summary>
    /// Delete a emaillog
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmailLog(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteEmailLogAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"EmailLog with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting emaillog with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the emaillog");
        }
    }
}
