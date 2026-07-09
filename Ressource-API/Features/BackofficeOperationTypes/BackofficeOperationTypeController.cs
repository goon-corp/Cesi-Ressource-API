using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.BackofficeOperationTypes.BackofficeOperationTypeDtos;
using Ressource_API.Features.BackofficeOperationTypes.Services;

namespace Ressource_API.Features.BackofficeOperationTypes;

[ApiController]
[Route("api/[controller]")]
public class BackofficeOperationTypeController : ControllerBase
{
    private readonly IBackofficeOperationTypeService _service;
    private readonly ILogger<BackofficeOperationTypeController> _logger;

    public BackofficeOperationTypeController(IBackofficeOperationTypeService service, ILogger<BackofficeOperationTypeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all backofficeoperationtypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BackofficeOperationType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BackofficeOperationType>>> GetAllBackofficeOperationTypes(CancellationToken cancellationToken)
    {
        try
        {
            var backofficeoperationtypes = await _service.GetAllBackofficeOperationTypesAsync(cancellationToken);
            return Ok(backofficeoperationtypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all backofficeoperationtypes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving backofficeoperationtypes");
        }
    }

    /// <summary>
    /// Get a backofficeoperationtype by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BackofficeOperationType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BackofficeOperationType>> GetBackofficeOperationTypeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var backofficeoperationtype = await _service.GetBackofficeOperationTypeByIdAsync(id, cancellationToken);

            if (backofficeoperationtype == null)
            {
                return NotFound($"BackofficeOperationType with ID {id} not found");
            }

            return Ok(backofficeoperationtype);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving backofficeoperationtype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the backofficeoperationtype");
        }
    }

    /// <summary>
    /// Create a new backofficeoperationtype
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BackofficeOperationType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeOperationType>> CreateBackofficeOperationType([FromBody] CreateBackofficeOperationTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdBackofficeOperationType = await _service.CreateBackofficeOperationTypeAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetBackofficeOperationTypeById),
                new { id = createdBackofficeOperationType.Id },
                createdBackofficeOperationType
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating backofficeoperationtype");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the backofficeoperationtype");
        }
    }

    /// <summary>
    /// Update an existing backofficeoperationtype
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BackofficeOperationType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BackofficeOperationType>> UpdateBackofficeOperationType(int id, [FromBody] UpdateBackofficeOperationTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBackofficeOperationType = await _service.UpdateBackofficeOperationTypeAsync(id, dto, cancellationToken);

            if (updatedBackofficeOperationType == null)
            {
                return NotFound($"BackofficeOperationType with ID {id} not found");
            }

            return Ok(updatedBackofficeOperationType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating backofficeoperationtype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the backofficeoperationtype");
        }
    }

    /// <summary>
    /// Delete a backofficeoperationtype
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBackofficeOperationType(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteBackofficeOperationTypeAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"BackofficeOperationType with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting backofficeoperationtype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the backofficeoperationtype");
        }
    }
}
