using Ressource_API.Features.Comments.Models;
using Ressource_API.Features.Comments.CommentDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Comments.Factories;

public class CommentFactory : BaseFactory<Comment>, ICommentFactory
{
    /// <summary>
    /// Creates a Comment from a DTO
    /// </summary>
    public Comment Create(CreateCommentDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Comment CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Comment
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateCommentDto dto)
        {
            // Create from DTO
            return new Comment
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Comment creation");
    }
}
