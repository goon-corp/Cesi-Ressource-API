using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;

namespace Ressource_API.Features.FriendsRequests.Repositories;

public interface IFriendsRequestRepository : IBaseRepository<FriendsRequest>
{
    Task<PaginatedList<FriendsRequestInfoDto>> PaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default);
    
    Task<FriendsRequest?> FindByUsersAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default);
}