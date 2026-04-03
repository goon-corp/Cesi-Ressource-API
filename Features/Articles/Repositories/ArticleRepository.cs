using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Articles.Dtos;

namespace Ressource_API.Features.Articles.Repositories;

public class ArticleRepository : BaseRepository<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Article?> GetArticle(Guid articleId, CancellationToken token = default)
    {
        return await _context.Articles.Include(a => a.Ressource).FirstOrDefaultAsync(a => a.Id == articleId, token);
    }

    public async Task<Article?> GetArticleNoTrackingByRessourceId(Guid ressourceId, CancellationToken token = default)
    {
        return await _context.Articles.AsNoTracking()
            .Include(a => a.Ressource)
                .ThenInclude(r => r.RessourceStatus)
            .Include(a => a.Ressource)
                .ThenInclude(r => r.RessourceConfidentialityType)
            .Include(a => a.Ressource)
                .ThenInclude(r => r.RessourceType)
            .Include(a => a.Ressource)
                .ThenInclude(r => r.Tags)
            .FirstOrDefaultAsync(a => a.RessourceId == ressourceId, token);
    }
}