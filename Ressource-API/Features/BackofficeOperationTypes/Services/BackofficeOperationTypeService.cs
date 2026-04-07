using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Features.BackofficeOperationTypes.BackofficeOperationTypeDtos;
using Ressource_API.Features.BackofficeOperationTypes.Repositories;
using Ressource_API.Features.BackofficeOperationTypes.Factories;

namespace Ressource_API.Features.BackofficeOperationTypes.Services;

public class BackofficeOperationTypeService : IBackofficeOperationTypeService
{
    private readonly IBackofficeOperationTypeRepository _repository;
    private readonly IBackofficeOperationTypeFactory _factory;

    public BackofficeOperationTypeService(
        IBackofficeOperationTypeRepository repository,
        IBackofficeOperationTypeFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<BackofficeOperationType>> GetAllBackofficeOperationTypesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<BackofficeOperationType?> GetBackofficeOperationTypeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<BackofficeOperationType> CreateBackofficeOperationTypeAsync(CreateBackofficeOperationTypeDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var backofficeoperationtype = _factory.Create(dto);
        
        return await _repository.AddAsync(backofficeoperationtype, cancellationToken);
    }

    public async Task<BackofficeOperationType?> UpdateBackofficeOperationTypeAsync(int id, UpdateBackofficeOperationTypeDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteBackofficeOperationTypeAsync(int id, CancellationToken cancellationToken = default)
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
