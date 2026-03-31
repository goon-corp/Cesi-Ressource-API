using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;
using Ressource_API.Features.Ressources.Query;

namespace Ressource_API.Features.Ressources.Repositories;

public class RessourceRepository : BaseRepository<Ressource>, IRessourceRepository
{
    public RessourceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Ressource?> FindWithTagsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Ressources
            .Include(r => r.Tags)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<PaginatedList<ReturnRessourceDto>> PaginatedRessourcesAsync(RessourceQuery query,
        CancellationToken cancellationToken = default)
    {
        var ressources = _context.Ressources.AsQueryable();

        // Filtre SQL:

        ressources = ressources.OrderByDescending(r => r.CreationTime)
            .Where(r => r.DeletionTime == null);

        if (!string.IsNullOrWhiteSpace(query.RessourceTitle))
        {
            ressources = ressources.Where(r => r.Title.Contains(query.RessourceTitle));
        }

        // filtrage par tag
        if (query.RessourceTags is { Count: > 0 })
        {
            ressources = ressources.Where(r => r.Tags.Any(t => query.RessourceTags.Contains(t.Id)));
        }

        if (!string.IsNullOrWhiteSpace(query.RessourceType))
        {
            ressources = ressources.Where(r => r.RessourceType.Label.Contains(query.RessourceType));
        }

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value;
            ressources = ressources.Where(r => r.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            ressources = (bool)query.IsDeleted
                ? ressources.Where(t => t.DeletionTime != null)
                : ressources.Where(t => t.DeletionTime == null);
        }

        var totalCount = await ressources.CountAsync(cancellationToken);

        // -------------------------
        // Pagination SQL
        // -------------------------
        var entities = await ressources
            .Include(r => r.RessourceStatus)
            .Include(r => r.RessourceConfidentialityType)
            .Include(r => r.RessourceType)
            .Include(r => r.Tags)
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ReturnRessourceDto>(
            entities.Select(r => r.ToReturnDto()).ToList(),
            query.page, query.size, totalCount);
    }
}