using Ressource_API.Features.PollOptions.Models;
using Ressource_API.Features.PollOptions.PollOptionDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PollOptions.Factories;

public class PollOptionFactory : BaseFactory<PollOption>, IPollOptionFactory
{
    /// <summary>
    /// Creates a PollOption from a DTO
    /// </summary>
    public PollOption Create(CreatePollOptionDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override PollOption CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new PollOption
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreatePollOptionDto dto)
        {
            // Create from DTO
            return new PollOption
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for PollOption creation");
    }
}
