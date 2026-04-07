using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.SessionDtos;
using Ressource_API.Features.Sessions.Repositories;
using Ressource_API.Features.Sessions.Factories;

namespace Ressource_API.Features.Sessions.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _repository;
    private readonly ISessionFactory _factory;

    public SessionService(
        ISessionRepository repository,
        ISessionFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Session>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<Session?> GetSessionByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Session> CreateSessionAsync(CreateSessionDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var session = _factory.Create(dto);
        
        return await _repository.AddAsync(session, cancellationToken);
    }

    public async Task<Session?> UpdateSessionAsync(int id, UpdateSessionDto dto, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteSessionAsync(int id, CancellationToken cancellationToken = default)
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
