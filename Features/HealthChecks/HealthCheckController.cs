using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.HealthChecks.Dtos;
using Ressource_API.Features.HealthChecks.Services;

namespace Ressource_API.Features.HealthChecks;

[ApiController]
[Route("/public")]
public class HealthCheckController : ControllerBase
{
    private readonly IHealthCheckService _healthCheckService;
    private readonly ILogger<HealthCheckController> _logger;

    public HealthCheckController(
        IHealthCheckService healthCheckService,
        ILogger<HealthCheckController> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    /// <summary>
    /// Check the health of the API and the database connection
    /// </summary>
    /// <remarks>
    /// This endpoint is only accessible to development environment
    /// </remarks>
    [HttpGet("/public/health-check")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HealthCheckDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthCheckDto), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> CheckHealth(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Health check asked from {IpAddress}", 
            HttpContext.Connection.RemoteIpAddress);

        var result = await _healthCheckService.CheckHealthAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable, 
                new 
                { 
                    message = result.Error,
                    status = "Unhealthy" 
                });
        }

        // Return 503 if unhealthy, else 200
        if (result.Data?.Status == "Unhealthy")
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable, 
                result.Data);
        }

        return Ok(result.Data);
    }
}