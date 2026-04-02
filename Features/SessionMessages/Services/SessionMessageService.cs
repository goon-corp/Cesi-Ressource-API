using System.Security.Claims;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.SessionMessages.Dtos;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.Repositories;

namespace Ressource_API.Features.SessionMessages.Services;

public class SessionMessageService : ISessionMessageService
{
    private readonly ISessionMessageRepository _repository;

    public SessionMessageService(ISessionMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ReturnSessionMessageDto>>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var messages = await _repository.GetBySessionIdAsync(sessionId, cancellationToken);
        return Result.Success(messages.Select(m => new ReturnSessionMessageDto
        {
            Id = m.Id,
            SentTime = m.SentTime,
            Content = m.Content,
            UserId = m.UserId,
            SessionId = m.SessionId,
        }).ToList());
    }

    public async Task<Result<ReturnSessionMessageDto>> GetSessionMessageByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await _repository.FindAsync(id, cancellationToken);
        if (message is null) return Result.Failure<ReturnSessionMessageDto>("Message not found");

        return Result.Success(new ReturnSessionMessageDto
        {
            Id = message.Id,
            SentTime = message.SentTime,
            Content = message.Content,
            UserId = message.UserId,
            SessionId = message.SessionId,
        });
    }

    public async Task<Result<ReturnSessionMessageDto>> CreateSessionMessageAsync(CreateSessionMessageDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Result.Failure<ReturnSessionMessageDto>("User not authenticated");

        var message = new SessionMessage
        {
            Id = Guid.CreateVersion7(),
            SentTime = DateTime.UtcNow,
            Content = dto.Content,
            SessionId = dto.SessionId,
            UserId = Guid.Parse(userId),
        };

        await _repository.AddAsync(message, cancellationToken);

        return Result.Success(new ReturnSessionMessageDto
        {
            Id = message.Id,
            SentTime = message.SentTime,
            Content = message.Content,
            UserId = message.UserId,
            SessionId = message.SessionId,
        });
    }

    public async Task<Result<ReturnSessionMessageDto>> UpdateSessionMessageAsync(Guid id, UpdateSessionMessageDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return Result.Failure<ReturnSessionMessageDto>("Message not found");

        existing.Content = dto.Content;

        await _repository.UpdateAsync(existing, cancellationToken);

        return Result.Success(new ReturnSessionMessageDto
        {
            Id = existing.Id,
            SentTime = existing.SentTime,
            Content = existing.Content,
            UserId = existing.UserId,
            SessionId = existing.SessionId,
        });
    }

    public async Task<Result> DeleteSessionMessageAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        if (existing is null) return Result.Failure("Message not found");

        await _repository.DeleteAsync(existing, cancellationToken);
        return Result.Success();
    }
}
