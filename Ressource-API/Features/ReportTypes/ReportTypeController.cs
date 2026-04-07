using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.Services;

namespace Ressource_API.Features.ReportTypes;

[ApiController]
[Route("api/report-types")]
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
    /// Get all report types
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<ReportType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<IEnumerable<ReportType>>>> GetAllReportTypes(
        CancellationToken cancellationToken)
    {
        var result = await _service.GetAllReportTypesAsync(cancellationToken);
        return result.Match<ActionResult>(
            onSuccess: data => Ok(data),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));
    }
}