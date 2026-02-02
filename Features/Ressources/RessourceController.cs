using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;
using Ressource_API.Features.Ressources.Services;

namespace Ressource_API.Features.Ressources;

[ApiController]
[Route("api/[controller]")]
public class RessourceController : ControllerBase
{
    private readonly IRessourceService _service;
    private readonly ILogger<RessourceController> _logger;

    public RessourceController(IRessourceService service, ILogger<RessourceController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressources
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Ressource>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ressource>>> GetAllRessources(CancellationToken cancellationToken)
    {
        try
        {
            var ressources = await _service.GetAllRessourcesAsync(cancellationToken);
            return Ok(ressources);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressources");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressources");
        }
    }

    /// <summary>
    /// Get a ressource by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Ressource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Ressource>> GetRessourceById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressource = await _service.GetRessourceByIdAsync(id, cancellationToken);

            if (ressource == null)
            {
                return NotFound($"Ressource with ID {id} not found");
            }

            return Ok(ressource);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressource with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressource");
        }
    }

    /// <summary>
    /// Create a new ressource
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Ressource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ressource>> CreateRessource([FromBody] CreateRessourceDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessource = await _service.CreateRessourceAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceById),
                new { id = createdRessource.Id },
                createdRessource
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressource");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressource");
        }
    }

    /// <summary>
    /// Update an existing ressource
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Ressource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Ressource>> UpdateRessource(int id, [FromBody] UpdateRessourceDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessource = await _service.UpdateRessourceAsync(id, dto, cancellationToken);

            if (updatedRessource == null)
            {
                return NotFound($"Ressource with ID {id} not found");
            }

            return Ok(updatedRessource);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressource with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressource");
        }
    }

    /// <summary>
    /// Delete a ressource
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessource(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Ressource with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressource with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressource");
        }
    }
}
