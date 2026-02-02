using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.ReportDtos;
using Ressource_API.Features.Reports.Services;

namespace Ressource_API.Features.Reports;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _service;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IReportService service, ILogger<ReportController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all reports
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Report>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Report>>> GetAllReports(CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _service.GetAllReportsAsync(cancellationToken);
            return Ok(reports);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all reports");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving reports");
        }
    }

    /// <summary>
    /// Get a report by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Report), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Report>> GetReportById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _service.GetReportByIdAsync(id, cancellationToken);

            if (report == null)
            {
                return NotFound($"Report with ID {id} not found");
            }

            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving report with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the report");
        }
    }

    /// <summary>
    /// Create a new report
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Report), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Report>> CreateReport([FromBody] CreateReportDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReport = await _service.CreateReportAsync(dto, cancellationToken);

            return CreatedAtAction(
                nameof(GetReportById),
                new { id = createdReport.Id },
                createdReport
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating report");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the report");
        }
    }

    /// <summary>
    /// Update an existing report
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Report), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Report>> UpdateReport(int id, [FromBody] UpdateReportDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedReport = await _service.UpdateReportAsync(id, dto, cancellationToken);

            if (updatedReport == null)
            {
                return NotFound($"Report with ID {id} not found");
            }

            return Ok(updatedReport);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating report with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the report");
        }
    }

    /// <summary>
    /// Delete a report
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteReportAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound($"Report with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting report with ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the report");
        }
    }
}
