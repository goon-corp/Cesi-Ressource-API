using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.Users.Query;

public class UserQuery : PagedQueryParameters
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? UserRoleId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public DateOnly? CreatedAt { get; set; }
}