using Ressource_API.Features.PollOptions.Extensions;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Extensions;

namespace Ressource_API.Features.Polls.Extensions;

public static class PollExtensions
{
    public static PollInfoDto ToInfoDto(this Poll poll)
    {
        return new PollInfoDto
        {
            Id = poll.Id,
            VoteCount = poll.VoteCount,
            Ressource = poll.Ressource.ToReturnDto(),
            Options = poll.PollsOptions.Select(o => o.ToInfoDto()).ToList()
        };
    }

    public static PollInfoDto ToInfoDto(this Poll poll, ReturnRessourceDto ressource)
    {
        return new PollInfoDto
        {
            Id = poll.Id,
            VoteCount = poll.VoteCount,
            Ressource = ressource,
            Options = poll.PollsOptions.Select(o => o.ToInfoDto()).ToList()
        };
    }
}
