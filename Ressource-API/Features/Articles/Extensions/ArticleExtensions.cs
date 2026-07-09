using Ressource_API.Features.Articles.Dtos;
using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;

namespace Ressource_API.Features.Articles.Extensions;

public static class ArticleExtensions
{
    extension(Article article)
    {
        public ReturnArticleDto ToReturnDto()
        {
            return new()
            {
                Id = article.Id,
                Content = article.Content,
                Ressource = article.Ressource.ToReturnDto()
            };
        }

        public ReturnArticleDto ToReturnDto(ReturnRessourceDto ressource)
        {
            return new()
            {
                Id = article.Id,
                Content = article.Content,
                Ressource = ressource
            };
        }
    }

    extension(CreateArticleDto dto)
    {
        public Article ToArticle(Guid ressourceId)
        {
            return new()
            {
                Id = Guid.CreateVersion7(),
                Content = dto.Content,
                RessourceId = ressourceId
            };
        }
    }
}