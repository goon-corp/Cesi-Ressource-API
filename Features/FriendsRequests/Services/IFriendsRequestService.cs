using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;

namespace Ressource_API.Features.FriendsRequests.Services;

public interface IFriendsRequestService
{
    Task<IEnumerable<FriendsRequest>> GetAllFriendsRequestsAsync(CancellationToken cancellationToken = default);
    Task<FriendsRequest?> GetFriendsRequestByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FriendsRequest> CreateFriendsRequestAsync(CreateFriendsRequestDto dto, CancellationToken cancellationToken = default);
    Task<FriendsRequest?> UpdateFriendsRequestAsync(int id, UpdateFriendsRequestDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteFriendsRequestAsync(int id, CancellationToken cancellationToken = default);
}
