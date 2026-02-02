using Ressource_API.Common.Data;
using Ressource_API.Features.Users.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Users.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
