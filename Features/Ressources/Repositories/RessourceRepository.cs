using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.Tags.Models;

namespace Ressource_API.Features.Ressources.Repositories;

public class RessourceRepository : BaseRepository<Ressource>, IRessourceRepository
{
    public RessourceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PaginatedList<Ressource>> PaginatedRessourcesAsync(RessourceQuery query, CancellationToken cancellationToken = default )
    {
        var ressources = _context.Ressources.Select(r => r);
        
        // Filtre SQL:

        ressources = ressources.OrderByDescending(r => r.CreationTime);

        if (!string.IsNullOrWhiteSpace(query.RessourceTitle))
        {
            ressources = ressources.Where(r => r.Title.Contains(query.RessourceTitle));
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
        var items = await ressources
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Ressource>(items, query.page, query.size, totalCount);
    }
}
