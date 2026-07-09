namespace Ressource_API.Features.QuizzQuestions.Dtos;

public class CreateQuizzQuestionDto
{
    public required string Question { get; set; }
    public required string PossibleAnswers { get; set; }
    public required string CorrectAnswer { get; set; }
    public Guid QuizzId { get; set; }
}
