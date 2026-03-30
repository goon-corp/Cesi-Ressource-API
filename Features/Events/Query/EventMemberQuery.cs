using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Events.Query;

public class EventMemberQuery : PagedQueryParameters
{
    public string? UserName { get; set; }
}
