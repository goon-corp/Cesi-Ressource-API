using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Models;

namespace Ressource_API.Features.PollOptions.Extensions;

public static class PollOptionExtensions
{
    public static PollOptionInfoDto ToInfoDto(this PollOption pollOption)
    {
        return new PollOptionInfoDto
        {
            Id = pollOption.Id,
            Option = pollOption.Option,
            PollId = pollOption.PollId,
            CreationTime = pollOption.CreationTime,
            UpdateTime = pollOption.UpdateTime,
            DeletionTime = pollOption.DeletionTime
        };
    }
}