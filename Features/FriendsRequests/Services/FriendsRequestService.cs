using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.Extensions;
using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Repositories;
using Ressource_API.Features.FriendsRequests.Factories;
using Ressource_API.Features.FriendsRequests.Query;

namespace Ressource_API.Features.FriendsRequests.Services;

public class FriendsRequestService : IFriendsRequestService
{
    private readonly IFriendsRequestRepository _repository;
    private readonly IFriendsRequestFactory _factory;

    public FriendsRequestService(
        IFriendsRequestRepository repository,
        IFriendsRequestFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<PaginatedList<FriendsRequestInfoDto>> GetPaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _repository.PaginatedFriendsRequestsAsync(query, cancellationToken);
    }

    public async Task<FriendsRequestInfoDto?> GetFriendsRequestByIdsAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        return existing?.ToInfoDto();
    }

    public async Task<FriendsRequestInfoDto> CreateFriendsRequestAsync(
        CreateFriendsRequestDto dto,
        Guid userSenderId,
        CancellationToken cancellationToken = default)
    {
        var friendsRequest = _factory.Create(dto, userSenderId);

        var created = await _repository.AddAsync(friendsRequest, cancellationToken);

        return created.ToInfoDto();
    }

    public async Task<FriendsRequestInfoDto?> UpdateFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        UpdateFriendsRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        if (existing == null)
            return null;

        existing.RequestStatus = dto.RequestStatus;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return existing.ToInfoDto();
    }

    public async Task<bool> DeleteFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        if (existing == null)
            return false;

        existing.DeletionTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return true;
    }
}