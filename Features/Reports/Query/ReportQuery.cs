using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Reports.Query;

public class ReportQuery : PagedQueryParameters
{
    public Guid? UserId { get; set; }
    public Guid? RessourceId { get; set; }
    public Guid? ReportTypeId { get; set; }
    public bool? IsCheckedByModerator { get; set; }
    public DateOnly? CreatedAt { get; set; }
}