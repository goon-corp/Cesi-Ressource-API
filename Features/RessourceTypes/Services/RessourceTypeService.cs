using Ressource_API.Features.RessourceTypes.Models;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Features.RessourceTypes.Repositories;
using Ressource_API.Features.RessourceTypes.Factories;

namespace Ressource_API.Features.RessourceTypes.Services;

public class RessourceTypeService : IRessourceTypeService
{
    private readonly IRessourceTypeRepository _repository;
    private readonly IRessourceTypeFactory _factory;

    public RessourceTypeService(
        IRessourceTypeRepository repository,
        IRessourceTypeFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RessourceType>> GetAllRessourceTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<RessourceType?> GetRessourceTypeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RessourceType> CreateRessourceTypeAsync(CreateRessourceTypeDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressourcetype = _factory.Create(dto);
        
        return await _repository.AddAsync(ressourcetype, cancellationToken);
    }

    public async Task<RessourceType?> UpdateRessourceTypeAsync(int id, UpdateRessourceTypeDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceTypeAsync(int id, CancellationToken cancellationToken = default)
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
