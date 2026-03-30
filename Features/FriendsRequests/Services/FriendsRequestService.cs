using Ressource_API.Common.Pagination;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.FriendsRequests.Extensions;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Factories;
using Ressource_API.Features.FriendsRequests.Query;
using Ressource_API.Features.FriendsRequests.Repositories;

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

    public async Task<Result<PaginatedList<FriendsRequestInfoDto>>> GetPaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.PaginatedFriendsRequestsAsync(query, cancellationToken);
        return Result.Success(result);
    }

    public async Task<Result<FriendsRequestInfoDto>> GetFriendsRequestByIdsAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        if (existing == null)
            return Result.Failure<FriendsRequestInfoDto>("FriendsRequest not found");

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result<FriendsRequestInfoDto>> CreateFriendsRequestAsync(
        CreateFriendsRequestDto dto,
        Guid userSenderId,
        CancellationToken cancellationToken = default)
    {
        var alreadyExists = await _repository.FindByUsersAsync(userSenderId, dto.UserReceiverId, cancellationToken);

        if (alreadyExists != null)
            return Result.Failure<FriendsRequestInfoDto>("A friends request already exists between these users");

        var friendsRequest = _factory.Create(dto, userSenderId);
        var created = await _repository.AddAsync(friendsRequest, cancellationToken);

        return Result.Success(created.ToInfoDto());
    }

    public async Task<Result<FriendsRequestInfoDto>> UpdateFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        UpdateFriendsRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        if (existing == null)
            return Result.Failure<FriendsRequestInfoDto>("FriendsRequest not found");

        existing.RequestStatus = dto.RequestStatus;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(existing.ToInfoDto());
    }

    public async Task<Result> DeleteFriendsRequestAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByUsersAsync(userSenderId, userReceiverId, cancellationToken);

        if (existing == null)
            return Result.Failure("FriendsRequest not found");

        existing.DeletionTime = DateTime.UtcNow;
        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success();
    }
}