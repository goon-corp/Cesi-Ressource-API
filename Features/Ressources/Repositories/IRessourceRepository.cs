using Ressource_API.Features.Ressources.Models;
using Ressource_API.Common.Data.Repositories;
using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Query;

namespace Ressource_API.Features.Ressources.Repositories;

public interface IRessourceRepository : IBaseRepository<Ressource>
{
    Task<PaginatedList<ReturnRessourceDto>> PaginatedRessourcesAsync(RessourceQuery query,
        CancellationToken cancellationToken = default);
}
