using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.ArticleDtos;

namespace Ressource_API.Features.Articles.Services;

public interface IArticleService
{
    Task<IEnumerable<Article>> GetAllArticlesAsync(CancellationToken cancellationToken = default);
    Task<Article?> GetArticleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Article> CreateArticleAsync(CreateArticleDto dto, CancellationToken cancellationToken = default);
    Task<Article?> UpdateArticleAsync(int id, UpdateArticleDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default);
}
