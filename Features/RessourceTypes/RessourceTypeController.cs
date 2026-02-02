using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Features.RessourceTypes.Services;

namespace Ressource_API.Features.RessourceTypes;

[ApiController]
[Route("api/[controller]")]
public class RessourceTypeController : ControllerBase
{
    private readonly IRessourceTypeService _service;
    private readonly ILogger<RessourceTypeController> _logger;

    public RessourceTypeController(IRessourceTypeService service, ILogger<RessourceTypeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourcetypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceType>>> GetAllRessourceTypes(CancellationToken cancellationToken)
    {
        try
        {
            var ressourcetypes = await _service.GetAllRessourceTypesAsync(cancellationToken);
            return Ok(ressourcetypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourcetypes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourcetypes");
        }
    }

    /// <summary>
    /// Get a ressourcetype by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RessourceType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceType>> GetRessourceTypeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressourcetype = await _service.GetRessourceTypeByIdAsync(id, cancellationToken);

            if (ressourcetype == null)
            {
                return NotFound($"RessourceType with ID {id} not found");
            }

            return Ok(ressourcetype);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressourcetype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressourcetype");
        }
    }

    /// <summary>
    /// Create a new ressourcetype
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceType>> CreateRessourceType([FromBody] CreateRessourceTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessourceType = await _service.CreateRessourceTypeAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceTypeById),
                new { id = createdRessourceType.Id },
                createdRessourceType
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourcetype");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressourcetype");
        }
    }

    /// <summary>
    /// Update an existing ressourcetype
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RessourceType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceType>> UpdateRessourceType(int id, [FromBody] UpdateRessourceTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessourceType = await _service.UpdateRessourceTypeAsync(id, dto, cancellationToken);

            if (updatedRessourceType == null)
            {
                return NotFound($"RessourceType with ID {id} not found");
            }

            return Ok(updatedRessourceType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourcetype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressourcetype");
        }
    }

    /// <summary>
    /// Delete a ressourcetype
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceType(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceTypeAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RessourceType with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourcetype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressourcetype");
        }
    }
}
