namespace Ressource_API.Features.HealthChecks.Dtos;

public class HealthCheckDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public double TotalDurationMs { get; set; }
    public List<HealthCheckEntry> Checks { get; set; } = new();
}

public record HealthCheckEntry(
    string Name,
    string Status,
    double DurationMs,
    string? Description = null,
    string? Error = null
);