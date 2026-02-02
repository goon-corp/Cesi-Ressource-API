using Ressource_API.Common.Data;
using Ressource_API.Features.ProfilePictures.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.ProfilePictures.Repositories;

public class ProfilePictureRepository : BaseRepository<ProfilePicture>, IProfilePictureRepository
{
    public ProfilePictureRepository(ApplicationDbContext context) : base(context)
    {
    }
}
