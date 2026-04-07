using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Features.SessionMessages.SessionMessageDtos;

namespace Ressource_API.Features.SessionMessages.Services;

public interface ISessionMessageService
{
    Task<IEnumerable<SessionMessage>> GetAllSessionMessagesAsync(CancellationToken cancellationToken = default);
    Task<SessionMessage?> GetSessionMessageByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SessionMessage> CreateSessionMessageAsync(CreateSessionMessageDto dto, CancellationToken cancellationToken = default);
    Task<SessionMessage?> UpdateSessionMessageAsync(int id, UpdateSessionMessageDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSessionMessageAsync(int id, CancellationToken cancellationToken = default);
}
