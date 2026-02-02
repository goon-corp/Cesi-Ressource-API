using Ressource_API.Common.Data;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Logins.Repositories;

public class LoginRepository : BaseRepository<Login>, ILoginRepository
{
    public LoginRepository(ApplicationDbContext context) : base(context)
    {
    }
}
