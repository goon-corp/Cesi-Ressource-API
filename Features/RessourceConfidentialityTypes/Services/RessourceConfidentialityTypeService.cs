using Ressource_API.Features.RessourceConfidentialityTypes.Models;
using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.RessourceConfidentialityTypes.Repositories;
using Ressource_API.Features.RessourceConfidentialityTypes.Factories;

namespace Ressource_API.Features.RessourceConfidentialityTypes.Services;

public class RessourceConfidentialityTypeService : IRessourceConfidentialityTypeService
{
    private readonly IRessourceConfidentialityTypeRepository _repository;
    private readonly IRessourceConfidentialityTypeFactory _factory;

    public RessourceConfidentialityTypeService(
        IRessourceConfidentialityTypeRepository repository,
        IRessourceConfidentialityTypeFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RessourceConfidentialityType>> GetAllRessourceConfidentialityTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<RessourceConfidentialityType?> GetRessourceConfidentialityTypeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RessourceConfidentialityType> CreateRessourceConfidentialityTypeAsync(CreateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressourceconfidentialitytype = _factory.Create(dto);
        
        return await _repository.AddAsync(ressourceconfidentialitytype, cancellationToken);
    }

    public async Task<RessourceConfidentialityType?> UpdateRessourceConfidentialityTypeAsync(int id, UpdateRessourceConfidentialityTypeDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceConfidentialityTypeAsync(int id, CancellationToken cancellationToken = default)
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
