using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.UserDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Users.Factories;

public class UserFactory : BaseFactory<User>, IUserFactory
{
    /// <summary>
    /// Creates a User from a DTO
    /// </summary>
    public User Create(CreateUserDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override User CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new User
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateUserDto dto)
        {
            // Create from DTO
            return new User
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for User creation");
    }
}
