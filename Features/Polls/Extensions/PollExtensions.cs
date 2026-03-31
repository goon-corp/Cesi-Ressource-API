using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;

namespace Ressource_API.Features.Polls.Extensions;

public static class PollExtensions
{
    public static PollInfoDto ToInfoDto(this Poll poll)
    {
        return new PollInfoDto
        {
            Id = poll.Id,
            VoteCount = poll.VoteCount,
            RessourceId = poll.RessourceId
        };
    }
}