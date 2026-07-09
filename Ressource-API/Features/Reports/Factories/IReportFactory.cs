using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Models;

namespace Ressource_API.Features.Reports.Factories;

public interface IReportFactory
{
    Report Create(CreateReportDto dto, Guid userId);
}