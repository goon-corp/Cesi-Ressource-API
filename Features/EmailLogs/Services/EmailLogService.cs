using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;
using Ressource_API.Features.EmailLogs.Repositories;
using Ressource_API.Features.EmailLogs.Factories;

namespace Ressource_API.Features.EmailLogs.Services;

public class EmailLogService : IEmailLogService
{
    private readonly IEmailLogRepository _repository;
    private readonly IEmailLogFactory _factory;

    public EmailLogService(
        IEmailLogRepository repository,
        IEmailLogFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<EmailLog?> GetEmailLogByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<EmailLog> CreateEmailLogAsync(CreateEmailLogDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var emaillog = _factory.Create(dto);
        
        return await _repository.AddAsync(emaillog, cancellationToken);
    }

    public async Task<EmailLog?> UpdateEmailLogAsync(int id, UpdateEmailLogDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteEmailLogAsync(int id, CancellationToken cancellationToken = default)
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
