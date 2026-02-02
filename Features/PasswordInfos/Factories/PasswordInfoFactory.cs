using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PasswordInfos.Factories;

public class PasswordInfoFactory : BaseFactory<PasswordInfo>, IPasswordInfoFactory
{
    /// <summary>
    /// Creates a PasswordInfo from a DTO
    /// </summary>
    public PasswordInfo Create(CreatePasswordInfoDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override PasswordInfo CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new PasswordInfo
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreatePasswordInfoDto dto)
        {
            // Create from DTO
            return new PasswordInfo
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for PasswordInfo creation");
    }
}
