using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.ArticleDtos;
using Ressource_API.Features.Articles.Repositories;
using Ressource_API.Features.Articles.Factories;

namespace Ressource_API.Features.Articles.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;
    private readonly IArticleFactory _factory;

    public ArticleService(
        IArticleRepository repository,
        IArticleFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<Article>> GetAllArticlesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Article?> GetArticleByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<Article> CreateArticleAsync(CreateArticleDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var article = _factory.Create(dto);
        
        return await _repository.AddAsync(article, cancellationToken);
    }

    public async Task<Article?> UpdateArticleAsync(int id, UpdateArticleDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
