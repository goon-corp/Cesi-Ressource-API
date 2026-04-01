namespace Ressource_API.Features.QuizzQuestions.Dtos;

public class QuizzQuestionInfoDto
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string PossibleAnswers { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public Guid QuizzId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public DateTime? DeletionTime { get; set; }
}