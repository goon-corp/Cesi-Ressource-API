using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceStatuses.Repositories;
using Ressource_API.Features.RessourceStatuses.Factories;

namespace Ressource_API.Features.RessourceStatuses.Services;

public class RessourceStatusService : IRessourceStatusService
{
    private readonly IRessourceStatusRepository _repository;
    private readonly IRessourceStatusFactory _factory;

    public RessourceStatusService(
        IRessourceStatusRepository repository,
        IRessourceStatusFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<RessourceStatus>> GetAllRessourceStatussAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<RessourceStatus?> GetRessourceStatusByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<RessourceStatus> CreateRessourceStatusAsync(CreateRessourceStatusDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var ressourcestatus = _factory.Create(dto);
        
        return await _repository.AddAsync(ressourcestatus, cancellationToken);
    }

    public async Task<RessourceStatus?> UpdateRessourceStatusAsync(int id, UpdateRessourceStatusDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteRessourceStatusAsync(int id, CancellationToken cancellationToken = default)
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
