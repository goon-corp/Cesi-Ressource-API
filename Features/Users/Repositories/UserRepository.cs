using Microsoft.EntityFrameworkCore;
using Ressource_API.Common.Data;
using Ressource_API.Features.Users.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Users.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<User?> FindWithUserRoleAsync(Guid userId)
    {
        return await _context.Set<User>()
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
