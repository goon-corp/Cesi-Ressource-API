using Ressource_API.Features.Sessions.Models;
using Ressource_API.Features.Sessions.SessionDtos;

namespace Ressource_API.Features.Sessions.Services;

public interface ISessionService
{
    Task<IEnumerable<Session>> GetAllSessionsAsync(CancellationToken cancellationToken = default);
    Task<Session?> GetSessionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Session> CreateSessionAsync(CreateSessionDto dto, CancellationToken cancellationToken = default);
    Task<Session?> UpdateSessionAsync(int id, UpdateSessionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSessionAsync(int id, CancellationToken cancellationToken = default);
}
