using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.Query;

namespace Ressource_API.Features.Reports.Repositories;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<PaginatedList<ReportInfoDto>> PaginatedReportsAsync(
        ReportQuery query,
        CancellationToken cancellationToken = default);

    Task<Report?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}