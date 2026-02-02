using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;
using Ressource_API.Features.PasswordHistories.Repositories;
using Ressource_API.Features.PasswordHistories.Factories;

namespace Ressource_API.Features.PasswordHistories.Services;

public class PasswordHistoryService : IPasswordHistoryService
{
    private readonly IPasswordHistoryRepository _repository;
    private readonly IPasswordHistoryFactory _factory;

    public PasswordHistoryService(
        IPasswordHistoryRepository repository,
        IPasswordHistoryFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<PasswordHistory>> GetAllPasswordHistorysAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<PasswordHistory?> GetPasswordHistoryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<PasswordHistory> CreatePasswordHistoryAsync(CreatePasswordHistoryDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var passwordhistory = _factory.Create(dto);
        
        return await _repository.AddAsync(passwordhistory, cancellationToken);
    }

    public async Task<PasswordHistory?> UpdatePasswordHistoryAsync(int id, UpdatePasswordHistoryDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeletePasswordHistoryAsync(int id, CancellationToken cancellationToken = default)
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
