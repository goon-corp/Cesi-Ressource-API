namespace Ressource_API.Features.PollOptions.Dtos;

public class UpdateNestedPollOptionDto
{
    public Guid? Id { get; set; }
    public required string Option { get; set; }
}
