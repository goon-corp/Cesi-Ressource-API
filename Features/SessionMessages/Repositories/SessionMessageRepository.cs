using Ressource_API.Common.Data;
using Ressource_API.Features.SessionMessages.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.SessionMessages.Repositories;

public class SessionMessageRepository : BaseRepository<SessionMessage>, ISessionMessageRepository
{
    public SessionMessageRepository(ApplicationDbContext context) : base(context)
    {
    }
}
