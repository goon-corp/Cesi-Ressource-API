using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.HealthChecks.Dtos;

namespace Ressource_API.Features.HealthChecks.Services;

public interface IHealthCheckService
{
    Task<Result<HealthCheckDto>> CheckHealthAsync(CancellationToken cancellationToken = default);
}

public record HealthCheckResponse(
    string Status,
    double TotalDurationMs,
    List<HealthCheckEntry> Checks
);