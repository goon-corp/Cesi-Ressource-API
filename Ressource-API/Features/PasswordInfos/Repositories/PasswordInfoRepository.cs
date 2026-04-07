using Ressource_API.Common.Data;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.PasswordInfos.Repositories;

public class PasswordInfoRepository : BaseRepository<PasswordInfo>, IPasswordInfoRepository
{
    public PasswordInfoRepository(ApplicationDbContext context) : base(context)
    {
    }
}
