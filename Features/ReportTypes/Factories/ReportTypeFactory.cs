using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.ReportTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.ReportTypes.Factories;

public class ReportTypeFactory : BaseFactory<ReportType>, IReportTypeFactory
{
    /// <summary>
    /// Creates a ReportType from a DTO
    /// </summary>
    public ReportType Create(CreateReportTypeDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override ReportType CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new ReportType
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateReportTypeDto dto)
        {
            // Create from DTO
            return new ReportType
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for ReportType creation");
    }
}
