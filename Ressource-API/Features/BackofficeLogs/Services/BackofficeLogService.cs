using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeLogs.BackofficeLogDtos;
using Ressource_API.Features.BackofficeLogs.Repositories;
using Ressource_API.Features.BackofficeLogs.Factories;

namespace Ressource_API.Features.BackofficeLogs.Services;

public class BackofficeLogService : IBackofficeLogService
{
    private readonly IBackofficeLogRepository _repository;
    private readonly IBackofficeLogFactory _factory;

    public BackofficeLogService(
        IBackofficeLogRepository repository,
        IBackofficeLogFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<BackofficeLog>> GetAllBackofficeLogsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<BackofficeLog?> GetBackofficeLogByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<BackofficeLog> CreateBackofficeLogAsync(CreateBackofficeLogDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var backofficelog = _factory.Create(dto);
        
        return await _repository.AddAsync(backofficelog, cancellationToken);
    }

    public async Task<BackofficeLog?> UpdateBackofficeLogAsync(int id, UpdateBackofficeLogDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteBackofficeLogAsync(int id, CancellationToken cancellationToken = default)
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
