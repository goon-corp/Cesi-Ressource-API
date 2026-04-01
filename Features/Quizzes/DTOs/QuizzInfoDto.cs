namespace Ressource_API.Features.Quizzes.Dtos;

public class QuizzInfoDto
{
    public Guid Id { get; set; }
    public long ParticipationCount { get; set; }
    public Guid RessourceId { get; set; }
}