using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.HealthChecks.Dtos;

namespace Ressource_API.Features.HealthChecks.Services;

public class HealthCheckService : IHealthCheckService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<HealthCheckService> _logger;

    public HealthCheckService(
        ApplicationDbContext dbContext,
        ILogger<HealthCheckService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<HealthCheckDto>> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var overallStopwatch = Stopwatch.StartNew();

        try
        {
            // Effectuer les vérifications
            var apiCheck = CheckApi();
            var databaseCheck = await CheckDatabaseAsync(cancellationToken);

            overallStopwatch.Stop();

            // Déterminer le statut global
            var checks = new List<HealthCheckEntry> { apiCheck, databaseCheck };
            var overallStatus = checks.Any(c => c.Status == "Unhealthy") ? "Unhealthy" : "Healthy";

            var healthCheckDto = new HealthCheckDto
            {
                Status = overallStatus,
                Timestamp = DateTime.UtcNow,
                TotalDurationMs = overallStopwatch.Elapsed.TotalMilliseconds,
                Checks = checks
            };

            return Result.Success(healthCheckDto);
        }
        catch (Exception ex)
        {
            overallStopwatch.Stop();
            _logger.LogError(ex, "Health check failed with exception");

            var unhealthyDto = new HealthCheckDto
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                TotalDurationMs = overallStopwatch.Elapsed.TotalMilliseconds,
                Checks = new List<HealthCheckEntry>
                {
                    new HealthCheckEntry(
                        Name: "overall",
                        Status: "Unhealthy",
                        DurationMs: overallStopwatch.Elapsed.TotalMilliseconds,
                        Error: ex.Message
                    )
                }
            };

            return Result.Success(unhealthyDto);
        }
    }

    private HealthCheckEntry CheckApi()
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Check memory usage
            var allocatedMemoryMb = GC.GetTotalMemory(false) / 1024 / 1024;
            const long maxMemoryMb = 1024; // 1 GB

            stopwatch.Stop();

            if (allocatedMemoryMb >= maxMemoryMb)
            {
                return new HealthCheckEntry(
                    Name: "api",
                    Status: "Unhealthy",
                    DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                    Description: null,
                    Error: $"Excessive memory: {allocatedMemoryMb} MB"
                );
            }

            return new HealthCheckEntry(
                Name: "api",
                Status: "Healthy",
                DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                Description: $"Memory: {allocatedMemoryMb} MB"
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error during API health check");

            return new HealthCheckEntry(
                Name: "api",
                Status: "Unhealthy",
                DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                Error: ex.Message
            );
        }
    }

    private async Task<HealthCheckEntry> CheckDatabaseAsync(CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Connection test
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                stopwatch.Stop();
                return new HealthCheckEntry(
                    Name: "database",
                    Status: "Unhealthy",
                    DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                    Error: "Unable to connect to database"
                );
            }

            // Read test (optional, but recommended)
            await _dbContext.Database.ExecuteSqlRawAsync(
                "SELECT 1",
                cancellationToken);

            stopwatch.Stop();

            return new HealthCheckEntry(
                Name: "database",
                Status: "Healthy",
                DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                Description: "Connection and read OK"
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error during database health check");

            return new HealthCheckEntry(
                Name: "database",
                Status: "Unhealthy",
                DurationMs: stopwatch.Elapsed.TotalMilliseconds,
                Error: ex.Message
            );
        }
    }
}