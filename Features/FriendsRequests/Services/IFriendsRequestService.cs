using System.Security.Claims;
using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;

namespace Ressource_API.Features.FriendsRequests.Services;

public interface IFriendsRequestService
{
    Task<Result<PaginatedList<FriendsRequestInfoDto>>> GetPaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default);

    Task<Result<FriendsRequestInfoDto>> GetFriendsRequestByIdsAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default);

    Task<Result<FriendsRequestInfoDto>> CreateFriendsRequestAsync(
        CreateFriendsRequestDto dto,
        // Guid userSenderId,
        ClaimsPrincipal context,
        CancellationToken cancellationToken = default);

    Task<Result<FriendsRequestInfoDto>> UpdateFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        UpdateFriendsRequestDto dto,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default);
}