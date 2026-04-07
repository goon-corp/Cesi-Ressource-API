namespace Ressource_API.Features.QuestionAnswers.Dtos;

public class CreateQuestionAnswerDto
{
    public string Answer { get; set; } = string.Empty;
    public Guid QuizzQuestionId { get; set; }
}