using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;

namespace Ressource_API.Features.Polls.Factories;

public class PollFactory : BaseFactory<Poll>, IPollFactory
{
    public Poll Create(CreatePollDto dto)
    {
        return CreateInstance(dto);
    }

    protected override Poll CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 1 && parameters[0] is CreatePollDto dto)
        {
            return new Poll
            {
                Id = Guid.NewGuid(),
                RessourceId = dto.RessourceId,
                VoteCount = 0
            };
        }

        throw new ArgumentException("Invalid parameters for Poll creation");
    }
}