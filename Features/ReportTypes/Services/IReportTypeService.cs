using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.ReportTypeDtos;

namespace Ressource_API.Features.ReportTypes.Services;

public interface IReportTypeService
{
    Task<IEnumerable<ReportType>> GetAllReportTypesAsync(CancellationToken cancellationToken = default);
    Task<ReportType?> GetReportTypeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ReportType> CreateReportTypeAsync(CreateReportTypeDto dto, CancellationToken cancellationToken = default);
    Task<ReportType?> UpdateReportTypeAsync(int id, UpdateReportTypeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteReportTypeAsync(int id, CancellationToken cancellationToken = default);
}
