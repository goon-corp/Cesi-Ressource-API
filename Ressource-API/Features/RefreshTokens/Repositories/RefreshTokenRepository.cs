using Ressource_API.Common.Data;
using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.RefreshTokens.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
