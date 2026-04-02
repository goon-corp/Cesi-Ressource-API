using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Quizzes.Dtos;

public class CreateQuizzDto
{
    public required CreateRessourceDto Ressource { get; set; }
}