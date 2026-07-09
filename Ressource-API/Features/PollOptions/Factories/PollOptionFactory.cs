using Ressource_API.Common.Data.Factories;
using Ressource_API.Features.PollOptions.Dtos;
using Ressource_API.Features.PollOptions.Models;

namespace Ressource_API.Features.PollOptions.Factories;

public class PollOptionFactory : BaseFactory<PollOption>, IPollOptionFactory
{
    public PollOption Create(CreatePollOptionDto dto)
    {
        return CreateInstance(dto);
    }

    protected override PollOption CreateInstance(params object[] parameters)
    {
        if (parameters.Length >= 1 && parameters[0] is CreatePollOptionDto dto)
        {
            return new PollOption
            {
                Id = Guid.NewGuid(),
                Option = dto.Option,
                PollId = dto.PollId,
                CreationTime = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for PollOption creation");
    }
}