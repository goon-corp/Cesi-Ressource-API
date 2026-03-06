using Ressource_API.Features.Logins.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.Logins.Repositories;

public interface ILoginRepository : IBaseRepository<Login>
{
    Task<Login?> GetLoginByUserId(Guid userId);
}
