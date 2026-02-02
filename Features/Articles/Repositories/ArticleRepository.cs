using Ressource_API.Common.Data;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Articles.Repositories;

public class ArticleRepository : BaseRepository<Article>, IArticleRepository
{
    public ArticleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
