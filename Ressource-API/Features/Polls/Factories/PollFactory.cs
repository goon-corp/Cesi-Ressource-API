using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.Polls.Dtos;
using Ressource_API.Features.Polls.Models;

namespace Ressource_API.Features.Polls.Factories;

public class PollFactory : BaseFactory<Poll>, IPollFactory
{
    public Poll Create(CreatePollDto dto, Guid ressourceId)
    {
        return CreateInstance(dto, ressourceId);
    }

    protected override Poll CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 2 && parameters[0] is CreatePollDto dto && parameters[1] is Guid ressourceId)
        {
            return new Poll
            {
                Id = Guid.NewGuid(),
                RessourceId = ressourceId,
                VoteCount = 0
            };
        }

        throw new ArgumentException("Invalid parameters for Poll creation");
    }
}