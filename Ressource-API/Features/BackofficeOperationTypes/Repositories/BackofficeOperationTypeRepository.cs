using Ressource_API.Common.Data;
using Ressource_API.Features.BackofficeOperationTypes.Models;
using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Features.BackofficeOperationTypes.Repositories;

public class BackofficeOperationTypeRepository : BaseRepository<BackofficeOperationType>, IBackofficeOperationTypeRepository
{
    public BackofficeOperationTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
