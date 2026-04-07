namespace Ressource_API.Features.QuizzQuestions.Dtos;

public class UpdateNestedQuizzQuestionDto
{
    public Guid? Id { get; set; }
    public required string Question { get; set; }
    public required string PossibleAnswers { get; set; }
    public required string CorrectAnswer { get; set; }
}
