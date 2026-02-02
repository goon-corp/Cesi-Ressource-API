using Ressource_API.Features.Articles.Models;
using Ressource_API.Features.Articles.ArticleDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Articles.Factories;

public interface IArticleFactory : IBaseFactory<Article>
{
    Article Create(CreateArticleDto dto);
}
