using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Articles.Dtos;

public class UpdateArticleDto
{
    public required string Content  { get; set; }
    public required UpdateRessourceDto Ressource { get; set; }
}
