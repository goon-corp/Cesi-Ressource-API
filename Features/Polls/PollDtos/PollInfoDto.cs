using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Polls.Dtos;

public class PollInfoDto
{
    public Guid Id { get; set; }
    public long VoteCount { get; set; }
    public required ReturnRessourceDto Ressource { get; set; }
    public List<PollOptionInfoDto> Options { get; set; } = [];
}
