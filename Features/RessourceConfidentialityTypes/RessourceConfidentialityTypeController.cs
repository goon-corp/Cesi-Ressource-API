using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.RessourceConfidentialityTypes.Services;

namespace Ressource_API.Features.RessourceConfidentialityTypes;

[ApiController]
[Route("api/[controller]")]
public class RessourceConfidentialityTypeController : ControllerBase
{
    private readonly IRessourceConfidentialityTypeService _service;
    private readonly ILogger<RessourceConfidentialityTypeController> _logger;

    public RessourceConfidentialityTypeController(IRessourceConfidentialityTypeService service, ILogger<RessourceConfidentialityTypeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourceconfidentialitytypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceConfidentialityType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceConfidentialityType>>> GetAllRessourceConfidentialityTypes(CancellationToken cancellationToken)
    {
        try
        {
            var ressourceconfidentialitytypes = await _service.GetAllRessourceConfidentialityTypesAsync(cancellationToken);
            return Ok(ressourceconfidentialitytypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourceconfidentialitytypes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourceconfidentialitytypes");
        }
    }

    /// <summary>
    /// Get a ressourceconfidentialitytype by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RessourceConfidentialityType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceConfidentialityType>> GetRessourceConfidentialityTypeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressourceconfidentialitytype = await _service.GetRessourceConfidentialityTypeByIdAsync(id, cancellationToken);

            if (ressourceconfidentialitytype == null)
            {
                return NotFound($"RessourceConfidentialityType with ID {id} not found");
            }

            return Ok(ressourceconfidentialitytype);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressourceconfidentialitytype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressourceconfidentialitytype");
        }
    }

    /// <summary>
    /// Create a new ressourceconfidentialitytype
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceConfidentialityType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceConfidentialityType>> CreateRessourceConfidentialityType([FromBody] CreateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessourceConfidentialityType = await _service.CreateRessourceConfidentialityTypeAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceConfidentialityTypeById),
                new { id = createdRessourceConfidentialityType.Id },
                createdRessourceConfidentialityType
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourceconfidentialitytype");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressourceconfidentialitytype");
        }
    }

    /// <summary>
    /// Update an existing ressourceconfidentialitytype
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RessourceConfidentialityType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceConfidentialityType>> UpdateRessourceConfidentialityType(int id, [FromBody] UpdateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessourceConfidentialityType = await _service.UpdateRessourceConfidentialityTypeAsync(id, dto, cancellationToken);

            if (updatedRessourceConfidentialityType == null)
            {
                return NotFound($"RessourceConfidentialityType with ID {id} not found");
            }

            return Ok(updatedRessourceConfidentialityType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourceconfidentialitytype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressourceconfidentialitytype");
        }
    }

    /// <summary>
    /// Delete a ressourceconfidentialitytype
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceConfidentialityType(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceConfidentialityTypeAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RessourceConfidentialityType with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourceconfidentialitytype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressourceconfidentialitytype");
        }
    }
}
