using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.RessourceMediaDtos;
using Ressource_API.Features.RessourceMedias.Services;

namespace Ressource_API.Features.RessourceMedias;

[ApiController]
[Route("api/[controller]")]
public class RessourceMediaController : ControllerBase
{
    private readonly IRessourceMediaService _service;
    private readonly ILogger<RessourceMediaController> _logger;

    public RessourceMediaController(IRessourceMediaService service, ILogger<RessourceMediaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourcemedias
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceMedia>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceMedia>>> GetAllRessourceMedias(CancellationToken cancellationToken)
    {
        try
        {
            var ressourcemedias = await _service.GetAllRessourceMediasAsync(cancellationToken);
            return Ok(ressourcemedias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourcemedias");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourcemedias");
        }
    }

    /// <summary>
    /// Get a ressourcemedia by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RessourceMedia), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceMedia>> GetRessourceMediaById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressourcemedia = await _service.GetRessourceMediaByIdAsync(id, cancellationToken);

            if (ressourcemedia == null)
            {
                return NotFound($"RessourceMedia with ID {id} not found");
            }

            return Ok(ressourcemedia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressourcemedia with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressourcemedia");
        }
    }

    /// <summary>
    /// Create a new ressourcemedia
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceMedia), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceMedia>> CreateRessourceMedia([FromBody] CreateRessourceMediaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessourceMedia = await _service.CreateRessourceMediaAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceMediaById),
                new { id = createdRessourceMedia.Id },
                createdRessourceMedia
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourcemedia");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressourcemedia");
        }
    }

    /// <summary>
    /// Update an existing ressourcemedia
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RessourceMedia), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceMedia>> UpdateRessourceMedia(int id, [FromBody] UpdateRessourceMediaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessourceMedia = await _service.UpdateRessourceMediaAsync(id, dto, cancellationToken);

            if (updatedRessourceMedia == null)
            {
                return NotFound($"RessourceMedia with ID {id} not found");
            }

            return Ok(updatedRessourceMedia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourcemedia with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressourcemedia");
        }
    }

    /// <summary>
    /// Delete a ressourcemedia
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceMedia(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceMediaAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RessourceMedia with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourcemedia with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressourcemedia");
        }
    }
}
