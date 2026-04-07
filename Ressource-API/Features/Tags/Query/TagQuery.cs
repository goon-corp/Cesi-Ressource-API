using System.Runtime.InteropServices.JavaScript;
using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Tags.Query;

public class TagQuery : PagedQueryParameters
{
    public string? TagName { get; set; }
    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }
}