using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Polls.Dtos;

public class UpdatePollDto
{
    public required UpdateRessourceDto Ressource { get; set; }
    public List<UpdateNestedPollOptionDto> Options { get; set; } = [];
}
