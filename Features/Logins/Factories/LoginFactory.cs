using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.Logins.Factories;

public class LoginFactory : BaseFactory<Login>, ILoginFactory
{
    /// <summary>
    /// Creates a Login from a DTO
    /// </summary>
    public Login Create(CreateLoginDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override Login CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new Login
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateLoginDto dto)
        {
            // Create from DTO
            return new Login
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for Login creation");
    }
}
