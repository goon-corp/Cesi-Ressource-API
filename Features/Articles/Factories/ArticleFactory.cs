using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.ArticleDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Articles.Factories;

public class ArticleFactory : BaseFactory<Article>, IArticleFactory
{
    /// <summary>
    /// Creates a Article from a DTO
    /// </summary>
    public Article Create(CreateArticleDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Article CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Article
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateArticleDto dto)
        {
            // Create from DTO
            return new Article
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Article creation");
    }
}
