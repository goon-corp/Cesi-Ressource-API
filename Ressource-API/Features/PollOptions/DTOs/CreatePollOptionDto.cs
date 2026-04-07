namespace Ressource_API.Features.PollOptions.Dtos;

public class CreatePollOptionDto
{
    public string Option { get; set; } = string.Empty;
    public Guid PollId { get; set; }
}
