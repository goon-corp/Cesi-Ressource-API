using Ressource_API.Features.Articles.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Articles.Dtos;

namespace Ressource_API.Features.Articles.Repositories;

public interface IArticleRepository : IBaseRepository<Article>
{
    Task<Article?> GetArticle(Guid articleId, CancellationToken token = default);

    Task<Article?> GetArticleNoTrackingByRessourceId(Guid ressourceId, CancellationToken token = default);
}
