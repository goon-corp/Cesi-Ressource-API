namespace Ressource_API.Features.QuizzQuestions.Dtos;

public class CreateQuizzQuestionDto
{
    public string Question { get; set; } = string.Empty;
    public string PossibleAnswers { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public Guid QuizzId { get; set; }
}