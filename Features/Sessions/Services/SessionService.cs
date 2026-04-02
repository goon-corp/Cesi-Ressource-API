using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Sessions.Dtos;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.Repositories;

namespace Ressource_API.Features.Sessions.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _repository;

    public SessionService(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ReturnSessionDto>>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _repository.ListAsync(cancellationToken);
        return Result.Success<IEnumerable<ReturnSessionDto>>(sessions.Select(s => new ReturnSessionDto
        {
            Id = s.Id,
            CreationTime = s.CreationTime,
            UpdateTime = s.UpdateTime,
            Status = s.Status,
            RessourceId = s.RessourceId,
        }));
    }

    public async Task<Result<ReturnSessionDto>> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _repository.FindAsync(id, cancellationToken);
        if (session is null) return Result.Failure<ReturnSessionDto>("Session not found");

        return Result.Success(new ReturnSessionDto
        {
            Id = session.Id,
            CreationTime = session.CreationTime,
            UpdateTime = session.UpdateTime,
            Status = session.Status,
            RessourceId = session.RessourceId,
        });
    }

    public async Task<Result<ReturnSessionDto>> CreateSessionAsync(CreateSessionDto dto, CancellationToken cancellationToken = default)
    {
        var session = new Session
        {
            Id = Guid.CreateVersion7(),
            CreationTime = DateTime.UtcNow,
            Status = dto.Status,
            IdWs = string.Empty,
            RessourceId = dto.RessourceId,
        };

        await _repository.AddAsync(session, cancellationToken);

        return Result.Success(new ReturnSessionDto
        {
            Id = session.Id,
            CreationTime = session.CreationTime,
            UpdateTime = session.UpdateTime,
            Status = session.Status,
            RessourceId = session.RessourceId,
        });
    }

    public async Task<Result<ReturnSessionDto>> UpdateSessionAsync(Guid id, UpdateSessionDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return Result.Failure<ReturnSessionDto>("Session not found");

        existing.Status = dto.Status;
        existing.UpdateTime = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(new ReturnSessionDto
        {
            Id = existing.Id,
            CreationTime = existing.CreationTime,
            UpdateTime = existing.UpdateTime,
            Status = existing.Status,
            RessourceId = existing.RessourceId,
        });
    }

    public async Task<Result> DeleteSessionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return Result.Failure("Session not found");

        await _repository.DeleteAsync(existing, cancellationToken);
        return Result.Success();
    }
}
