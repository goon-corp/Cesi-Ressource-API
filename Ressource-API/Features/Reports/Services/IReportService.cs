using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Query;

namespace Ressource_API.Features.Reports.Services;

public interface IReportService
{
    Task<Result<PaginatedList<ReportInfoDto>>> GetPaginatedReportsAsync(
        ReportQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<ReportInfoDto>> GetReportByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Result<ReportInfoDto>> CreateReportAsync(
        CreateReportDto dto,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    Task<Result<ReportInfoDto>> UpdateReportAsync(
        Guid id,
        UpdateReportDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteReportAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}