using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;
using Ressource_API.Features.RessourceProgressions.Repositories;
using Ressource_API.Features.RessourceProgressions.Factories;

namespace Ressource_API.Features.RessourceProgressions.Services;

public class RessourceProgressionService : IRessourceProgressionService
{
    private readonly IRessourceProgressionRepository _repository;
    private readonly IRessourceProgressionFactory _factory;

    public RessourceProgressionService(
        IRessourceProgressionRepository repository,
        IRessourceProgressionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RessourceProgression>> GetAllRessourceProgressionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<RessourceProgression?> GetRessourceProgressionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RessourceProgression> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressourceprogression = _factory.Create(dto);
        
        return await _repository.AddAsync(ressourceprogression, cancellationToken);
    }

    public async Task<RessourceProgression?> UpdateRessourceProgressionAsync(int id, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceProgressionAsync(int id, CancellationToken cancellationToken = default)
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
