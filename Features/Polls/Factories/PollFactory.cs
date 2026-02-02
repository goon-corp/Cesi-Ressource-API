using Ressource_API.Features.Polls.Models;
using Ressource_API.Features.Polls.PollDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Polls.Factories;

public class PollFactory : BaseFactory<Poll>, IPollFactory
{
    /// <summary>
    /// Creates a Poll from a DTO
    /// </summary>
    public Poll Create(CreatePollDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Poll CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Poll
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreatePollDto dto)
        {
            // Create from DTO
            return new Poll
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Poll creation");
    }
}
