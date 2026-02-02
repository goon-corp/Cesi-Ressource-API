using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.ProfilePictures.ProfilePictureDtos;

namespace Ressource_API.Features.ProfilePictures.Services;

public interface IProfilePictureService
{
    Task<IEnumerable<ProfilePicture>> GetAllProfilePicturesAsync(CancellationToken cancellationToken = default);
    Task<ProfilePicture?> GetProfilePictureByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProfilePicture> CreateProfilePictureAsync(CreateProfilePictureDto dto, CancellationToken cancellationToken = default);
    Task<ProfilePicture?> UpdateProfilePictureAsync(int id, UpdateProfilePictureDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProfilePictureAsync(int id, CancellationToken cancellationToken = default);
}
