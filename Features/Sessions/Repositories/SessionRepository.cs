using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Sessions.Repositories;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Session?> FindWithMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Include(s => s.SessionsMessages)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
