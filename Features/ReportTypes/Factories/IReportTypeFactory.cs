using Ressource_API.Features.ReportTypes.Models;
using Ressource_API.Features.ReportTypes.ReportTypeDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.ReportTypes.Factories;

public interface IReportTypeFactory : IBaseFactory<ReportType>
{
    ReportType Create(CreateReportTypeDto dto);
}
