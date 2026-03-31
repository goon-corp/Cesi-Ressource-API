using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Polls.Query;

public class PollQuery : PagedQueryParameters
{
    public Guid? RessourceId { get; set; }
    public bool? IsDeleted { get; set; }
}