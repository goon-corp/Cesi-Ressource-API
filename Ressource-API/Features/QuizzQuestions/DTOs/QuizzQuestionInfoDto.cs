namespace Ressource_API.Features.QuizzQuestions.Dtos;

public class QuizzQuestionInfoDto
{
    public Guid Id { get; set; }
    public required string Question { get; set; }
    public required string PossibleAnswers { get; set; }
    public required string CorrectAnswer { get; set; }
    public Guid QuizzId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public DateTime? DeletionTime { get; set; }
}
