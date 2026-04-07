using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogLevels.BackofficeLogLevelDtos;
using Ressource_API.Features.BackofficeLogLevels.Repositories;
using Ressource_API.Features.BackofficeLogLevels.Factories;

namespace Ressource_API.Features.BackofficeLogLevels.Services;

public class BackofficeLogLevelService : IBackofficeLogLevelService
{
    private readonly IBackofficeLogLevelRepository _repository;
    private readonly IBackofficeLogLevelFactory _factory;

    public BackofficeLogLevelService(
        IBackofficeLogLevelRepository repository,
        IBackofficeLogLevelFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<BackofficeLogLevel>> GetAllBackofficeLogLevelsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<BackofficeLogLevel?> GetBackofficeLogLevelByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<BackofficeLogLevel> CreateBackofficeLogLevelAsync(CreateBackofficeLogLevelDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var backofficeloglevel = _factory.Create(dto);
        
        return await _repository.AddAsync(backofficeloglevel, cancellationToken);
    }

    public async Task<BackofficeLogLevel?> UpdateBackofficeLogLevelAsync(int id, UpdateBackofficeLogLevelDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteBackofficeLogLevelAsync(int id, CancellationToken cancellationToken = default)
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
