namespace Ressource_API.Features.Reports.Dtos;

public class ReportInfoDto
{
    public Guid Id { get; set; }
    public Guid ReportTypeId { get; set; }
    public Guid UserId { get; set; }
    public Guid RessourceId { get; set; }
    public bool IsCheckedByModerator { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
}