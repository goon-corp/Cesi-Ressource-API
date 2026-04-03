using Ressource_API.Features.PollOptions.Dtos;

namespace Ressource_API.Features.Polls.Dtos;

public class PollInfoDto
{
    public Guid Id { get; set; }
    public long VoteCount { get; set; }
    public Guid RessourceId { get; set; }
    public List<PollOptionInfoDto> Options { get; set; } = [];
}
