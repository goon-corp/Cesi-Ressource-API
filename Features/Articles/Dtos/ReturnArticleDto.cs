using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Articles.Dtos;

public class ReturnArticleDto
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public required ReturnRessourceDto Ressource { get; set; }
}