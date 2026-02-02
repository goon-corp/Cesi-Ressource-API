using Ressource_API.Features.Tags.Models;
using Ressource_API.Features.Tags.TagDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Tags.Factories;

public class TagFactory : BaseFactory<Tag>, ITagFactory
{
    /// <summary>
    /// Creates a Tag from a DTO
    /// </summary>
    public Tag Create(CreateTagDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Tag CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Tag
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateTagDto dto)
        {
            // Create from DTO
            return new Tag
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Tag creation");
    }
}
