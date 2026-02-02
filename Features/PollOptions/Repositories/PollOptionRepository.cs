using Ressource_API.Common.Data;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.PollOptions.Repositories;

public class PollOptionRepository : BaseRepository<PollOption>, IPollOptionRepository
{
    public PollOptionRepository(ApplicationDbContext context) : base(context)
    {
    }
}
