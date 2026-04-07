using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Models;

namespace Ressource_API.Features.Reports.Factories;

public class ReportFactory : BaseFactory<Report>, IReportFactory
{
    public Report Create(CreateReportDto dto, Guid userId)
    {
        return CreateInstance(dto, userId);
    }

    protected override Report CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 2
            && parameters[0] is CreateReportDto dto
            && parameters[1] is Guid userId)
        {
            return new Report
            {
                Id = Guid.NewGuid(),
                ReportTypeId = dto.ReportTypeId,
                RessourceId = dto.RessourceId,
                UserId = userId,
                IsCheckedByModerator = false,
                CreationTime = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Report creation");
    }
}