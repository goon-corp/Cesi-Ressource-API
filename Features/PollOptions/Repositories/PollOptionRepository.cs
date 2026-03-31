using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Extensions;
using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.Query;

namespace Ressource_API.Features.PollOptions.Repositories;

public class PollOptionRepository : BaseRepository<PollOption>, IPollOptionRepository
{
    public PollOptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<PollOptionInfoDto>> PaginatedPollOptionsAsync(
        PollOptionQuery query,
        CancellationToken cancellationToken = default)
    {
        var pollOptions = _context.PollsOptions
            .AsQueryable()
            .OrderByDescending(po => po.CreationTime)
            .Where(po => po.DeletionTime == null);

        if (query.PollId.HasValue)
            pollOptions = pollOptions.Where(po => po.PollId == query.PollId.Value);

        if (query.IsDeleted is not null)
        {
            pollOptions = (bool)query.IsDeleted
                ? pollOptions.Where(po => po.DeletionTime != null)
                : pollOptions.Where(po => po.DeletionTime == null);
        }

        var totalCount = await pollOptions.CountAsync(cancellationToken);

        var entities = await pollOptions
            .Include(po => po.Poll)
            .Include(po => po.Users)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<PollOptionInfoDto>(
            entities.Select(po => po.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<PollOption?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PollsOptions
            .Include(po => po.Poll)
            .Include(po => po.Users)
            .FirstOrDefaultAsync(po => po.Id == id && po.DeletionTime == null, cancellationToken);
    }
}