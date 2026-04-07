using Ressource_API.Features.Reports.Dtos;
using Ressource_API.Features.Reports.Models;

namespace Ressource_API.Features.Reports.Extensions;

public static class ReportExtensions
{
    public static ReportInfoDto ToInfoDto(this Report report)
    {
        return new ReportInfoDto
        {
            Id = report.Id,
            ReportTypeId = report.ReportTypeId,
            UserId = report.UserId,
            RessourceId = report.RessourceId,
            IsCheckedByModerator = report.IsCheckedByModerator,
            CreationTime = report.CreationTime,
            UpdateTime = report.UpdateTime
        };
    }
}