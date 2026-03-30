using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;

namespace Ressource_API.Features.FriendsRequests.Services;

public interface IFriendsRequestService
{
    Task<PaginatedList<FriendsRequestInfoDto>> GetPaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default);

    Task<FriendsRequestInfoDto?> GetFriendsRequestByIdsAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default);

    Task<FriendsRequestInfoDto> CreateFriendsRequestAsync(
        CreateFriendsRequestDto dto,
        Guid userSenderId,
        CancellationToken cancellationToken = default);

    Task<FriendsRequestInfoDto?> UpdateFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        UpdateFriendsRequestDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default);
}