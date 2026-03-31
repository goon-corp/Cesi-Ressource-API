using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.PollOptions.Query;

public class PollOptionQuery : PagedQueryParameters
{
    public Guid? PollId { get; set; }
    public bool? IsDeleted { get; set; }
}