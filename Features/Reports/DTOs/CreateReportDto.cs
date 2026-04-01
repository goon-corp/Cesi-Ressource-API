namespace Ressource_API.Features.Reports.Dtos;

public class CreateReportDto
{
    public Guid ReportTypeId { get; set; }
    public Guid RessourceId { get; set; }
}