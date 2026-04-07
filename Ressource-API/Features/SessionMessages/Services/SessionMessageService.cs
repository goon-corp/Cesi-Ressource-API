using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.SessionMessageDtos;
using Ressource_API.Features.SessionMessages.Repositories;
using Ressource_API.Features.SessionMessages.Factories;

namespace Ressource_API.Features.SessionMessages.Services;

public class SessionMessageService : ISessionMessageService
{
    private readonly ISessionMessageRepository _repository;
    private readonly ISessionMessageFactory _factory;

    public SessionMessageService(
        ISessionMessageRepository repository,
        ISessionMessageFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<SessionMessage>> GetAllSessionMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<SessionMessage?> GetSessionMessageByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<SessionMessage> CreateSessionMessageAsync(CreateSessionMessageDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var sessionmessage = _factory.Create(dto);
        
        return await _repository.AddAsync(sessionmessage, cancellationToken);
    }

    public async Task<SessionMessage?> UpdateSessionMessageAsync(int id, UpdateSessionMessageDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteSessionMessageAsync(int id, CancellationToken cancellationToken = default)
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
