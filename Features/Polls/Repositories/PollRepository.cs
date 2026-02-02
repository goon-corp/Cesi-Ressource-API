using Ressource_API.Common.Data;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Polls.Repositories;

public class PollRepository : BaseRepository<Poll>, IPollRepository
{
    public PollRepository(ApplicationDbContext context) : base(context)
    {
    }
}
