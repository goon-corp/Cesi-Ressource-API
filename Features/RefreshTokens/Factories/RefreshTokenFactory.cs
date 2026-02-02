using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.RefreshTokens.Factories;

public class RefreshTokenFactory : BaseFactory<RefreshToken>, IRefreshTokenFactory
{
    /// <summary>
    /// Creates a RefreshToken from a DTO
    /// </summary>
    public RefreshToken Create(CreateRefreshTokenDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override RefreshToken CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new RefreshToken
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateRefreshTokenDto dto)
        {
            // Create from DTO
            return new RefreshToken
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for RefreshToken creation");
    }
}
