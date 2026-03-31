using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Query;
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
    /// Get all reports (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<ReportInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ReportInfoDto>>> GetPaginatedReports(
        [FromQuery] ReportQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginatedReportsAsync(query, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }

    /// <summary>
    /// Get a report by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReportInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReportInfoDto>> GetReportById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetReportByIdAsync(id, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Create a new report
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReportInfoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReportInfoDto>> CreateReport(
        [FromBody] CreateReportDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.CreateReportAsync(dto, User, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => CreatedAtAction(
                nameof(GetReportById),
                new { id = data.Id },
                data),
            onFailure: error => Unauthorized(error));
    }

    /// <summary>
    /// Update a report (moderator only)
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ReportInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReportInfoDto>> UpdateReport(
        Guid id,
        [FromBody] UpdateReportDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _service.UpdateReportAsync(id, dto, cancellationToken);

        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => NotFound(error));
    }

    /// <summary>
    /// Delete a report
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReport(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteReportAsync(id, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(error));
    }
}