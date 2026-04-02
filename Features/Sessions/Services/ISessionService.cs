using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Sessions.Dtos;

namespace Ressource_API.Features.Sessions.Services;

public interface ISessionService
{
    Task<Result<IEnumerable<ReturnSessionDto>>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionDto>> GetSessionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionDto>> CreateSessionAsync(CreateSessionDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReturnSessionDto>> UpdateSessionAsync(Guid id, UpdateSessionDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteSessionAsync(Guid id, CancellationToken cancellationToken = default);
}
