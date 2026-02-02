using Ressource_API.Features.FriendsRequests.Models;
using Ressource_API.Features.FriendsRequests.FriendsRequestDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.FriendsRequests.Factories;

public class FriendsRequestFactory : BaseFactory<FriendsRequest>, IFriendsRequestFactory
{
    /// <summary>
    /// Creates a FriendsRequest from a DTO
    /// </summary>
    public FriendsRequest Create(CreateFriendsRequestDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override FriendsRequest CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new FriendsRequest
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateFriendsRequestDto dto)
        {
            // Create from DTO
            return new FriendsRequest
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for FriendsRequest creation");
    }
}
