using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.ReportTypeDtos;
using Ressource_API.Features.ReportTypes.Services;

namespace Ressource_API.Features.ReportTypes;

[ApiController]
[Route("api/[controller]")]
public class ReportTypeController : ControllerBase
{
    private readonly IReportTypeService _service;
    private readonly ILogger<ReportTypeController> _logger;

    public ReportTypeController(IReportTypeService service, ILogger<ReportTypeController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all reporttypes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReportType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReportType>>> GetAllReportTypes(CancellationToken cancellationToken)
    {
        try
        {
            var reporttypes = await _service.GetAllReportTypesAsync(cancellationToken);
            return Ok(reporttypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all reporttypes");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving reporttypes");
        }
    }

    /// <summary>
    /// Get a reporttype by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReportType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportType>> GetReportTypeById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var reporttype = await _service.GetReportTypeByIdAsync(id, cancellationToken);

            if (reporttype == null)
            {
                return NotFound($"ReportType with ID {id} not found");
            }

            return Ok(reporttype);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reporttype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the reporttype");
        }
    }

    /// <summary>
    /// Create a new reporttype
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReportType>> CreateReportType([FromBody] CreateReportTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReportType = await _service.CreateReportTypeAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetReportTypeById),
                new { id = createdReportType.Id },
                createdReportType
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reporttype");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the reporttype");
        }
    }

    /// <summary>
    /// Update an existing reporttype
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ReportType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReportType>> UpdateReportType(int id, [FromBody] UpdateReportTypeDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedReportType = await _service.UpdateReportTypeAsync(id, dto, cancellationToken);

            if (updatedReportType == null)
            {
                return NotFound($"ReportType with ID {id} not found");
            }

            return Ok(updatedReportType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reporttype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the reporttype");
        }
    }

    /// <summary>
    /// Delete a reporttype
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReportType(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteReportTypeAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"ReportType with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reporttype with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the reporttype");
        }
    }
}
