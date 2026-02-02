using Ressource_API.Common.Data;
using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.PasswordHistories.Repositories;

public class PasswordHistoryRepository : BaseRepository<PasswordHistory>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
