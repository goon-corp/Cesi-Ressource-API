using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Features.UserRoles.UserRoleDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.UserRoles.Factories;

public class UserRoleFactory : BaseFactory<UserRole>, IUserRoleFactory
{
    /// <summary>
    /// Creates a UserRole from a DTO
    /// </summary>
    public UserRole Create(CreateUserRoleDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override UserRole CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new UserRole
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateUserRoleDto dto)
        {
            // Create from DTO
            return new UserRole
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for UserRole creation");
    }
}
