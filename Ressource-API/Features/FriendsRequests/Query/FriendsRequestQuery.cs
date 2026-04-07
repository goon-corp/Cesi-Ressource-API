using Ressource_API.Common.Pagination;

namespace Ressource_API.Features.FriendsRequests.Query;

public class FriendsRequestQuery : PagedQueryParameters
{
    public Guid? UserSenderId { get; set; }
    public Guid? UserReceiverId { get; set; }
    public string? RequestStatus { get; set; }
    public DateOnly? CreatedAt { get; set; }
    public bool? IsDeleted { get; set; }
}