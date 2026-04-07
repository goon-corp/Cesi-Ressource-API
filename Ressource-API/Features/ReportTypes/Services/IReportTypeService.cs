using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.ReportTypes.Models;

namespace Ressource_API.Features.ReportTypes.Services;

public interface IReportTypeService
{
    Task<Result<IEnumerable<ReportType>>> GetAllReportTypesAsync(CancellationToken cancellationToken = default);
}
