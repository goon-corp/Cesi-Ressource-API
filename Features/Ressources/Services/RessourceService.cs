using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Factories;

namespace Ressource_API.Features.Ressources.Services;

public class RessourceService : IRessourceService
{
    private readonly IRessourceRepository _repository;
    private readonly IRessourceFactory _factory;

    public RessourceService(
        IRessourceRepository repository,
        IRessourceFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Ressource>> GetAllRessourcesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            string tagIds = "Tags";
            string userId = "User";
            var ressources = await _repository.ListAsync(new List<string>() { tagIds, userId }, cancellationToken);
            return ressources;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<Ressource?> GetRessourceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Ressource> CreateRessourceAsync(CreateRessourceDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressource = _factory.Create(dto);
        
        return await _repository.AddAsync(ressource, cancellationToken);
    }

    public async Task<Ressource?> UpdateRessourceAsync(int id, UpdateRessourceDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceAsync(int id, CancellationToken cancellationToken = default)
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
