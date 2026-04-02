using System.Security.Claims;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.SessionMessages.Dtos;

namespace Ressource_API.Features.SessionMessages.Services;

public interface ISessionMessageService
{
    Task<Result<List<ReturnSessionMessageDto>>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionMessageDto>> GetSessionMessageByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionMessageDto>> CreateSessionMessageAsync(CreateSessionMessageDto dto, ClaimsPrincipal user, CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionMessageDto>> UpdateSessionMessageAsync(Guid id, UpdateSessionMessageDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteSessionMessageAsync(Guid id, CancellationToken cancellationToken = default);
}
