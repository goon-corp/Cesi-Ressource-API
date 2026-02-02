using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Repositories;
using Ressource_API.Features.FriendsRequests.Factories;

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

    public async Task<IEnumerable<FriendsRequest>> GetAllFriendsRequestsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<FriendsRequest?> GetFriendsRequestByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<FriendsRequest> CreateFriendsRequestAsync(CreateFriendsRequestDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var friendsrequest = _factory.Create(dto);
        
        return await _repository.AddAsync(friendsrequest, cancellationToken);
    }

    public async Task<FriendsRequest?> UpdateFriendsRequestAsync(int id, UpdateFriendsRequestDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteFriendsRequestAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
