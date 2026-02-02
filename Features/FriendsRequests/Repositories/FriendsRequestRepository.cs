using Ressource_API.Common.Data;
using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.FriendsRequests.Repositories;

public class FriendsRequestRepository : BaseRepository<FriendsRequest>, IFriendsRequestRepository
{
    public FriendsRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
}
