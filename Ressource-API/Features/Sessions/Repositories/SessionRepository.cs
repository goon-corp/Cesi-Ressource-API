using Ressource_API.Common.Data;
using Ressource_API.Features.Sessions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Sessions.Repositories;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
