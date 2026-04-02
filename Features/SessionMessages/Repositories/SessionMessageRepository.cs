using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.SessionMessages.Repositories;

public class SessionMessageRepository : BaseRepository<SessionMessage>, ISessionMessageRepository
{
    public SessionMessageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<SessionMessage>> GetBySessionIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await _context.SessionsMessages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.SentTime)
            .ToListAsync(cancellationToken);
    }
}
