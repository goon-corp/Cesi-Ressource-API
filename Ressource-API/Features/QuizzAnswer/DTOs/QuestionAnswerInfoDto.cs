namespace Ressource_API.Features.QuestionAnswers.Dtos;

public class QuestionAnswerInfoDto
{
    public Guid UserId { get; set; }
    public Guid QuizzQuestionId { get; set; }
    public string Answer { get; set; } = string.Empty;
}