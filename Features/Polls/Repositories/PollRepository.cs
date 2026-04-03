using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Extensions;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.Query;

namespace Ressource_API.Features.Polls.Repositories;

public class PollRepository : BaseRepository<Poll>, IPollRepository
{
    public PollRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<PollInfoDto>> PaginatedPollsAsync(
        PollQuery query,
        CancellationToken cancellationToken = default)
    {
        var polls = _context.Polls.AsQueryable();

        if (query.RessourceId.HasValue)
            polls = polls.Where(p => p.RessourceId == query.RessourceId.Value);

        var totalCount = await polls.CountAsync(cancellationToken);

        var entities = await polls
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceStatus)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceConfidentialityType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.Tags)
            .Include(p => p.PollsOptions)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<PollInfoDto>(
            entities.Select(p => p.ToInfoDto()).ToList(),
            query.page, query.size, totalCount);
    }

    public async Task<Poll?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Polls
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceStatus)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceConfidentialityType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.Tags)
            .Include(p => p.PollsOptions)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Poll?> GetPollNoTrackingByRessourceId(
        Guid ressourceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Polls.AsNoTracking()
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceStatus)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceConfidentialityType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.RessourceType)
            .Include(p => p.Ressource)
                .ThenInclude(r => r.Tags)
            .Include(p => p.PollsOptions)
            .FirstOrDefaultAsync(p => p.RessourceId == ressourceId, cancellationToken);
    }
}
