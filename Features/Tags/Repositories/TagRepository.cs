using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Tags.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Tags.Query;

namespace Ressource_API.Features.Tags.Repositories;

public class TagRepository : BaseRepository<Tag>, ITagRepository
{
    private readonly DbContext _context;
    public TagRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<PaginatedList<Tag>> PaginatedListAsync(
        TagQuery query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Tag> tags = _context.Set<Tag>().AsQueryable();

        // -------------------------
        // Filtering (SQL)
        // -------------------------

        if (!string.IsNullOrWhiteSpace(query.TagName))
        {
            tags = tags.Where(t => t.Label.Contains(query.TagName));
        }

        if (query.CreatedAt.HasValue)
        {
            var date = query.CreatedAt.Value.Date;
            tags = tags.Where(t => t.CreationTime.Date == date);
        }

        if (query.IsDeleted is not null)
        {
            tags = (bool)query.IsDeleted
                ? tags.Where(t => t.DeletionTime != null)
                : tags.Where(t => t.DeletionTime == null);
        }

        // -------------------------
        // Pagination SQL
        // -------------------------

        var totalCount = await tags.CountAsync(cancellationToken);

        var items = await tags
            .Skip((query.page - 1) * query.size)
            .Take(query.size)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Tag>(items, query.page, query.size, totalCount);
    }

}
