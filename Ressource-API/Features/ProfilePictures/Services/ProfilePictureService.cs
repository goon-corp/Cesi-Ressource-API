using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Features.ProfilePictures.ProfilePictureDtos;
using Ressource_API.Features.ProfilePictures.Repositories;
using Ressource_API.Features.ProfilePictures.Factories;

namespace Ressource_API.Features.ProfilePictures.Services;

public class ProfilePictureService : IProfilePictureService
{
    private readonly IProfilePictureRepository _repository;
    private readonly IProfilePictureFactory _factory;

    public ProfilePictureService(
        IProfilePictureRepository repository,
        IProfilePictureFactory factory)
    {
        _repository = repository;
        _factory = factory;
    }

    public async Task<IEnumerable<ProfilePicture>> GetAllProfilePicturesAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.ListAsync(cancellationToken);
    }

    public async Task<ProfilePicture?> GetProfilePictureByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.FindAsync(id, cancellationToken);
    }

    public async Task<ProfilePicture> CreateProfilePictureAsync(CreateProfilePictureDto dto, CancellationToken cancellationToken = default)
    {
        // Use factory to create the entity from DTO
        var profilepicture = _factory.Create(dto);
        
        return await _repository.AddAsync(profilepicture, cancellationToken);
    }

    public async Task<ProfilePicture?> UpdateProfilePictureAsync(int id, UpdateProfilePictureDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return null;
        }

        // TODO: Map properties from dto to existing
        // Example: existing.Name = dto.Name;
        
        await _repository.UpdateAsync(existing, cancellationToken);
        
        return existing;
    }

    public async Task<bool> DeleteProfilePictureAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindAsync(id, cancellationToken);
        
        if (existing == null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        
        return true;
    }
}
