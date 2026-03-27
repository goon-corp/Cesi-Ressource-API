using Ressource_API.Features.Cities.Models;
using Ressource_API.Features.Cities.CityDtos;
using Ressource_API.Features.Cities.Repositories;
using Ressource_API.Features.Cities.Factories;

namespace Ressource_API.Features.Cities.Services;

public class Citieservice : ICitieservice
{
    private readonly ICityRepository _repository;
    private readonly ICityFactory _factory;

    public Citieservice(
        ICityRepository repository,
        ICityFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<City>> GetAllCitiesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<City?> GetCityByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<City> CreateCityAsync(CreateCityDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var city = _factory.Create(dto);
        
        return await _repository.AddAsync(city, cancellationToken);
    }

    public async Task<City?> UpdateCityAsync(int id, UpdateCityDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteCityAsync(int id, CancellationToken cancellationToken = default)
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
