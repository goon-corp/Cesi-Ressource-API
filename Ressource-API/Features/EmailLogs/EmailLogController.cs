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
    /// Get all emailLogs
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
            _logger.LogError(ex, "Error retrieving all emailLogs");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving emaillogs");
        }
    }

    /// <summary>
    /// Get an emailLog by ID
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
}
