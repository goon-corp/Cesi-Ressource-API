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
    /// Récupère toutes les progressions
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
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// Récupère une progression par le couple RessourceId et UserId
    /// </summary>
    [HttpGet("{ressourceId}/{userId}")]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceProgression>> GetRessourceProgressionById(Guid ressourceId, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var ressourceprogression = await _service.GetRessourceProgressionByIdAsync(ressourceId, userId, cancellationToken);

            if (ressourceprogression == null)
            {
                return NotFound($"Progression for Ressource {ressourceId} and User {userId} not found");
            }

            return Ok(ressourceprogression);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving progression");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// Crée une nouvelle progression
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status201Created)]
    public async Task<ActionResult<RessourceProgression>> CreateRessourceProgression([FromBody] CreateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateRessourceProgressionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceProgressionById),
                new { ressourceId = created.RessourceId, userId = created.UserId },
                created
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourceprogression");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// Met à jour une progression existante
    /// </summary>
    [HttpPut("{ressourceId}/{userId}")]
    [ProducesResponseType(typeof(RessourceProgression), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceProgression>> UpdateRessourceProgression(Guid ressourceId, Guid userId, [FromBody] UpdateRessourceProgressionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _service.UpdateRessourceProgressionAsync(ressourceId, userId, dto, cancellationToken);

            if (updated == null)
            {
                return NotFound("Progression not found");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourceprogression");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }

    /// <summary>
    /// Supprime une progression
    /// </summary>
    [HttpDelete("{ressourceId}/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceProgression(Guid ressourceId, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceProgressionAsync(ressourceId, userId, cancellationToken);

            if (!deleted)
            {
                return NotFound("Progression not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourceprogression");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }
}