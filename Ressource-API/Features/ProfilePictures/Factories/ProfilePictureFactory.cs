using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.ProfilePictures.ProfilePictureDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.ProfilePictures.Factories;

public class ProfilePictureFactory : BaseFactory<ProfilePicture>, IProfilePictureFactory
{
    /// <summary>
    /// Creates a ProfilePicture from a DTO
    /// </summary>
    public ProfilePicture Create(CreateProfilePictureDto dto)
    {
        return CreateInstance(dto);
    }

    /// <summary>
    /// Implementation of the abstract CreateInstance method
    /// </summary>
    protected override ProfilePicture CreateInstance(params object[] parameters)
    {
        if (parameters.Length == 0)
        {
            // Create default instance
            return new ProfilePicture
            {
                // TODO: Set default values
                // Example: CreatedAt = DateTime.UtcNow
            };
        }

        if (parameters[0] is CreateProfilePictureDto dto)
        {
            // Create from DTO
            return new ProfilePicture
            {
                // TODO: Map DTO properties to entity
                // Example:
                // Name = dto.Name,
                // Description = dto.Description,
                // CreatedAt = DateTime.UtcNow
            };
        }

        throw new ArgumentException("Invalid parameters for ProfilePicture creation");
    }
}
