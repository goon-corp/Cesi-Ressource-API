namespace Ressource_API.Features.PollOptions.Dtos;

public class PollOptionInfoDto
{
    public Guid Id { get; set; }
    public string Option { get; set; } = string.Empty;
    public Guid PollId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public DateTime? DeletionTime { get; set; }
}