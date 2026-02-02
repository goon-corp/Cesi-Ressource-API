using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.ReportDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Reports.Factories;

public class ReportFactory : BaseFactory<Report>, IReportFactory
{
    /// <summary>
    /// Creates a Report from a DTO
    /// </summary>
    public Report Create(CreateReportDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Report CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Report
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateReportDto dto)
        {
            // Create from DTO
            return new Report
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Report creation");
    }
}
