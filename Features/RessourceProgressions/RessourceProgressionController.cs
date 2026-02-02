using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Features.RessourceProgressions.Services;

namespace Ressource_API.Features.RessourceProgressions;

[ApiController]
[Route("api/[controller]")]
public class RessourceProgressionController : ControllerBase
{
    private readonly IRessourceProgressionService _service;
    private readonly ILogger<RessourceProgressionController> _logger;

    public RessourceProgressionController(IRessourceProgressionService service, ILogger<RessourceProgressionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourceprogressions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceProgression>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceProgression>>> GetAllRessourceProgressions(CancellationToken cancellationToken)
    {
        try
        {
            var ressourceprogressions = await _service.GetAllRessourceProgressionsAsync(cancellationToken);
            return Ok(ressourceprogressions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourceprogressions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourceprogressions");
        }
    }

    /// <summary>
    /// Get a ressourceprogression by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceProgression>> GetRessourceProgressionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressourceprogression = await _service.GetRessourceProgressionByIdAsync(id, cancellationToken);

            if (ressourceprogression == null)
            {
                return NotFound($"RessourceProgression with ID {id} not found");
            }

            return Ok(ressourceprogression);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressourceprogression with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressourceprogression");
        }
    }

    /// <summary>
    /// Create a new ressourceprogression
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceProgression>> CreateRessourceProgression([FromBody] CreateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessourceProgression = await _service.CreateRessourceProgressionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceProgressionById),
                new { id = createdRessourceProgression.RessourceId },
                createdRessourceProgression
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourceprogression");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressourceprogression");
        }
    }

    /// <summary>
    /// Update an existing ressourceprogression
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceProgression>> UpdateRessourceProgression(int id, [FromBody] UpdateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessourceProgression = await _service.UpdateRessourceProgressionAsync(id, dto, cancellationToken);

            if (updatedRessourceProgression == null)
            {
                return NotFound($"RessourceProgression with ID {id} not found");
            }

            return Ok(updatedRessourceProgression);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourceprogression with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressourceprogression");
        }
    }

    /// <summary>
    /// Delete a ressourceprogression
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceProgression(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceProgressionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RessourceProgression with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourceprogression with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressourceprogression");
        }
    }
}
