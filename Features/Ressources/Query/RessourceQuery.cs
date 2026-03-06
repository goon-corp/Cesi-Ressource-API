using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Ressources.Query;

public class RessourceQuery : PagedQueryParameters
{
    
    public bool? IsDeleted { get; set; }
    
    public string? RessourceTitle {get; set;}
    
    public string? RessourceType {get; set;}
    
    public DateTime? CreatedAt { get; set; }
}