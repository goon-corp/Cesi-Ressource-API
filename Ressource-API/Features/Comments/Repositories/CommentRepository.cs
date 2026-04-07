using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Comments.Models;

namespace Ressource_API.Features.Comments.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<Comment> Items, int Total)> ListByRessourceAsync(Guid ressourceId, int page, int size, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Comment>()
            .Include(c => c.User)
            .Where(c => c.RessourceId == ressourceId)
            .OrderByDescending(c => c.CreationTime);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (items, total);
    }
}
