using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.ProfilePictures.ProfilePictureDtos;
using Ressource_API.Common.Data.Factories;

namespace Ressource_API.Features.ProfilePictures.Factories;

public interface IProfilePictureFactory : IBaseFactory<ProfilePicture>
{
    ProfilePicture Create(CreateProfilePictureDto dto);
}
