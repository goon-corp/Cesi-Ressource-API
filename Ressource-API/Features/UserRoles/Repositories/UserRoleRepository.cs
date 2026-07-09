using Ressource_API.Common.Data;
using Ressource_API.Features.UserRoles.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.UserRoles.Repositories;

public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
