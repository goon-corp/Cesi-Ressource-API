using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Quizzes.Dtos;

public class UpdateQuizzDto
{
    public required UpdateRessourceDto Ressource { get; set; }
    public List<UpdateNestedQuizzQuestionDto> Questions { get; set; } = [];
}