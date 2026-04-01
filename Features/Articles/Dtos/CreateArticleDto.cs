using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Articles.Dtos;

public class CreateArticleDto
{
    public required string Content  { get; set; }
    public required CreateRessourceDto Ressource { get; set; }
}
