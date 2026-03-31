namespace Ressource_API.Features.Polls.Dtos;

public class PollInfoDto
{
    public Guid Id { get; set; }
    public long VoteCount { get; set; }
    public Guid RessourceId { get; set; }
}