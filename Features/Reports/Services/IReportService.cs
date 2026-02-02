using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.ReportDtos;

namespace Ressource_API.Features.Reports.Services;

public interface IReportService
{
    Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken = default);
    Task<Report?> GetReportByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Report> CreateReportAsync(CreateReportDto dto, CancellationToken cancellationToken = default);
    Task<Report?> UpdateReportAsync(int id, UpdateReportDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteReportAsync(int id, CancellationToken cancellationToken = default);
}
