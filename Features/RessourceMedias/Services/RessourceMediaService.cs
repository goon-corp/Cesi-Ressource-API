using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.RessourceMediaDtos;
using Ressource_API.Features.RessourceMedias.Repositories;
using Ressource_API.Features.RessourceMedias.Factories;

namespace Ressource_API.Features.RessourceMedias.Services;

public class RessourceMediaService : IRessourceMediaService
{
    private readonly IRessourceMediaRepository _repository;
    private readonly IRessourceMediaFactory _factory;

    public RessourceMediaService(
        IRessourceMediaRepository repository,
        IRessourceMediaFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RessourceMedia>> GetAllRessourceMediasAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<RessourceMedia?> GetRessourceMediaByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RessourceMedia> CreateRessourceMediaAsync(CreateRessourceMediaDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressourcemedia = _factory.Create(dto);
        
        return await _repository.AddAsync(ressourcemedia, cancellationToken);
    }

    public async Task<RessourceMedia?> UpdateRessourceMediaAsync(int id, UpdateRessourceMediaDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceMediaAsync(int id, CancellationToken cancellationToken = default)
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
