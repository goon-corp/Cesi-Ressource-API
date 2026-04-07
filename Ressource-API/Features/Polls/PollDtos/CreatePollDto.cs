using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Polls.Dtos;

public class CreatePollDto
{
    public required CreateRessourceDto Ressource { get; set; }
    public List<CreateNestedPollOptionDto> Options { get; set; } = new List<CreateNestedPollOptionDto>();
}
