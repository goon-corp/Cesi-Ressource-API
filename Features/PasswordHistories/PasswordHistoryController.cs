using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;
using Ressource_API.Features.PasswordHistories.Services;

namespace Ressource_API.Features.PasswordHistories;

[ApiController]
[Route("api/[controller]")]
public class PasswordHistoryController : ControllerBase
{
    private readonly IPasswordHistoryService _service;
    private readonly ILogger<PasswordHistoryController> _logger;

    public PasswordHistoryController(IPasswordHistoryService service, ILogger<PasswordHistoryController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all passwordhistorys
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PasswordHistory>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasswordHistory>>> GetAllPasswordHistorys(CancellationToken cancellationToken)
    {
        try
        {
            var passwordhistorys = await _service.GetAllPasswordHistorysAsync(cancellationToken);
            return Ok(passwordhistorys);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all passwordhistorys");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving passwordhistorys");
        }
    }

    /// <summary>
    /// Get a passwordhistory by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PasswordHistory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PasswordHistory>> GetPasswordHistoryById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var passwordhistory = await _service.GetPasswordHistoryByIdAsync(id, cancellationToken);

            if (passwordhistory == null)
            {
                return NotFound($"PasswordHistory with ID {id} not found");
            }

            return Ok(passwordhistory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving passwordhistory with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the passwordhistory");
        }
    }

    /// <summary>
    /// Create a new passwordhistory
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PasswordHistory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordHistory>> CreatePasswordHistory([FromBody] CreatePasswordHistoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPasswordHistory = await _service.CreatePasswordHistoryAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetPasswordHistoryById),
                new { id = createdPasswordHistory.Id },
                createdPasswordHistory
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating passwordhistory");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the passwordhistory");
        }
    }

    /// <summary>
    /// Update an existing passwordhistory
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PasswordHistory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordHistory>> UpdatePasswordHistory(int id, [FromBody] UpdatePasswordHistoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPasswordHistory = await _service.UpdatePasswordHistoryAsync(id, dto, cancellationToken);

            if (updatedPasswordHistory == null)
            {
                return NotFound($"PasswordHistory with ID {id} not found");
            }

            return Ok(updatedPasswordHistory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating passwordhistory with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the passwordhistory");
        }
    }

    /// <summary>
    /// Delete a passwordhistory
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePasswordHistory(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeletePasswordHistoryAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"PasswordHistory with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting passwordhistory with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the passwordhistory");
        }
    }
}
