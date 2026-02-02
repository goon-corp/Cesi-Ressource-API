using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceStatuses.Services;

namespace Ressource_API.Features.RessourceStatuses;

[ApiController]
[Route("api/[controller]")]
public class RessourceStatusController : ControllerBase
{
    private readonly IRessourceStatusService _service;
    private readonly ILogger<RessourceStatusController> _logger;

    public RessourceStatusController(IRessourceStatusService service, ILogger<RessourceStatusController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all ressourcestatuss
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RessourceStatus>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RessourceStatus>>> GetAllRessourceStatuss(CancellationToken cancellationToken)
    {
        try
        {
            var ressourcestatuss = await _service.GetAllRessourceStatussAsync(cancellationToken);
            return Ok(ressourcestatuss);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ressourcestatuss");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving ressourcestatuss");
        }
    }

    /// <summary>
    /// Get a ressourcestatus by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RessourceStatus), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RessourceStatus>> GetRessourceStatusById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var ressourcestatus = await _service.GetRessourceStatusByIdAsync(id, cancellationToken);

            if (ressourcestatus == null)
            {
                return NotFound($"RessourceStatus with ID {id} not found");
            }

            return Ok(ressourcestatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ressourcestatus with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the ressourcestatus");
        }
    }

    /// <summary>
    /// Create a new ressourcestatus
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RessourceStatus), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceStatus>> CreateRessourceStatus([FromBody] CreateRessourceStatusDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRessourceStatus = await _service.CreateRessourceStatusAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRessourceStatusById),
                new { id = createdRessourceStatus.Id },
                createdRessourceStatus
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ressourcestatus");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the ressourcestatus");
        }
    }

    /// <summary>
    /// Update an existing ressourcestatus
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RessourceStatus), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RessourceStatus>> UpdateRessourceStatus(int id, [FromBody] UpdateRessourceStatusDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRessourceStatus = await _service.UpdateRessourceStatusAsync(id, dto, cancellationToken);

            if (updatedRessourceStatus == null)
            {
                return NotFound($"RessourceStatus with ID {id} not found");
            }

            return Ok(updatedRessourceStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ressourcestatus with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ressourcestatus");
        }
    }

    /// <summary>
    /// Delete a ressourcestatus
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRessourceStatus(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRessourceStatusAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"RessourceStatus with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ressourcestatus with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the ressourcestatus");
        }
    }
}
