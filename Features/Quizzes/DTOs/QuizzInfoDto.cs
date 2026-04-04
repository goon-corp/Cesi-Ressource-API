using Ressource_API.Features.QuizzQuestions.Dtos;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Quizzes.Dtos;

public class QuizzInfoDto
{
    public Guid Id { get; set; }
    public long ParticipationCount { get; set; }
    public required ReturnRessourceDto Ressource { get; set; }
    public List<QuizzQuestionInfoDto> Questions { get; set; } = [];
}
