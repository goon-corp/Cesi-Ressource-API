using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.PasswordHistories.Factories;

public class PasswordHistoryFactory : BaseFactory<PasswordHistory>, IPasswordHistoryFactory
{
    /// <summary>
    /// Creates a PasswordHistory from a DTO
    /// </summary>
    public PasswordHistory Create(CreatePasswordHistoryDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override PasswordHistory CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new PasswordHistory
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreatePasswordHistoryDto dto)
        {
            // Create from DTO
            return new PasswordHistory
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for PasswordHistory creation");
    }
}
