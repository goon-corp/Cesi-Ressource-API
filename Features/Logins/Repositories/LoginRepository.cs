using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Features.Users.Models;

namespace Ressource_API.Features.Logins.Repositories;

public class LoginRepository : BaseRepository<Login>, ILoginRepository
{
    public LoginRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Login?> GetLoginByUserId(Guid userId)
    {
        return await _context.Set<Login>().FirstOrDefaultAsync(l => l.UserId == userId);
    }
}
