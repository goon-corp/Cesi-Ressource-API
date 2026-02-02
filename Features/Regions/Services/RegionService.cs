using Ressource_API.Features.Regions.Models;
using Ressource_API.Features.Regions.RegionDtos;
using Ressource_API.Features.Regions.Repositories;
using Ressource_API.Features.Regions.Factories;

namespace Ressource_API.Features.Regions.Services;

public class RegionService : IRegionService
{
    private readonly IRegionRepository _repository;
    private readonly IRegionFactory _factory;

    public RegionService(
        IRegionRepository repository,
        IRegionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Region>> GetAllRegionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Region?> GetRegionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Region> CreateRegionAsync(CreateRegionDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var region = _factory.Create(dto);
        
        return await _repository.AddAsync(region, cancellationToken);
    }

    public async Task<Region?> UpdateRegionAsync(int id, UpdateRegionDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRegionAsync(int id, CancellationToken cancellationToken = default)
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
