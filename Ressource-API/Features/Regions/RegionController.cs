using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Regions.RegionDtos;
using Ressource_API.Features.Regions.Services;

namespace Ressource_API.Features.Regions;

[ApiController]
[Route("api/[controller]")]
public class RegionController : ControllerBase
{
    private readonly IRegionService _service;
    private readonly ILogger<RegionController> _logger;

    public RegionController(IRegionService service, ILogger<RegionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all regions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Region>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Region>>> GetAllRegions(CancellationToken cancellationToken)
    {
        try
        {
            var regions = await _service.GetAllRegionsAsync(cancellationToken);
            return Ok(regions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all regions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving regions");
        }
    }

    /// <summary>
    /// Get a region by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Region>> GetRegionById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var region = await _service.GetRegionByIdAsync(id, cancellationToken);

            if (region == null)
            {
                return NotFound($"Region with ID {id} not found");
            }

            return Ok(region);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving region with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the region");
        }
    }

    /// <summary>
    /// Create a new region
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Region), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Region>> CreateRegion([FromBody] CreateRegionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRegion = await _service.CreateRegionAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetRegionById),
                new { id = createdRegion.Id },
                createdRegion
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating region");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the region");
        }
    }

    /// <summary>
    /// Update an existing region
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Region), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Region>> UpdateRegion(int id, [FromBody] UpdateRegionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRegion = await _service.UpdateRegionAsync(id, dto, cancellationToken);

            if (updatedRegion == null)
            {
                return NotFound($"Region with ID {id} not found");
            }

            return Ok(updatedRegion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating region with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the region");
        }
    }

    /// <summary>
    /// Delete a region
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRegion(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteRegionAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Region with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting region with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the region");
        }
    }
}
