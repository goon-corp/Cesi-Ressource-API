using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.FriendsRequests.Extensions;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Features.FriendsRequests.Query;

namespace Ressource_API.Features.FriendsRequests.Repositories;

public class FriendsRequestRepository : BaseRepository<FriendsRequest>, IFriendsRequestRepository
{
    public FriendsRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<FriendsRequest?> FindByUsersAsync(
        Guid userSenderId,
        Guid userReceiverId,
        CancellationToken cancellationToken = default)
    {
        return await _context.FriendsRequests
            .FirstOrDefaultAsync(
                fr => fr.UserSenderId == userSenderId && fr.UserReceiverId == userReceiverId,
                cancellationToken);
    }

    public async Task<PaginatedList<FriendsRequestInfoDto>> PaginatedFriendsRequestsAsync(
        FriendsRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        var friendsRequests = _context.FriendsRequests.AsQueryable();

        // Filtre SQL :

        friendsRequests = friendsRequests
            .OrderByDescending(fr => fr.CreationTime)
            .Where(fr => fr.DeletionTime == null);

        if (query.UserSenderId.HasValue)
        {
            friendsRequests = friendsRequests.Where(fr => fr.UserSenderId == query.UserSenderId.Value);
        }

        if (query.UserReceiverId.HasValue)
        {
            friendsRequests = friendsRequests.Where(fr => fr.UserReceiverId == query.UserReceiverId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.RequestStatus))
        {
            friendsRequests = friendsRequests.Where(fr => fr.RequestStatus == query.RequestStatus);
        }

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value.ToDateTime(new TimeOnly());
            friendsRequests = friendsRequests.Where(fr => fr.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            friendsRequests = (bool)query.IsDeleted
                ? friendsRequests.Where(fr => fr.DeletionTime != null)
                : friendsRequests.Where(fr => fr.DeletionTime == null);
        }

        var totalCount = await friendsRequests.CountAsync(cancellationToken);

        // -------------------------
        // Pagination SQL
        // -------------------------
        var entities = await friendsRequests
            .Include(fr => fr.UserSender)
            .Include(fr => fr.UserReceiver)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<FriendsRequestInfoDto>(
            entities.Select(fr => fr.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }
}
