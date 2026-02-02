using Ressource_API.Features.Reports.Models;
using Ressource_API.Features.Reports.ReportDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Reports.Factories;

public interface IReportFactory : IBaseFactory<Report>
{
    Report Create(CreateReportDto dto);
}
