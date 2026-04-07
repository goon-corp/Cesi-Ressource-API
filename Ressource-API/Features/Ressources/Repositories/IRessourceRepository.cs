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

    Task<Ressource?> FindWithTagsAsync(Guid id, CancellationToken cancellationToken = default);

    // Returns true = now liked, false = now unliked, null = ressource or user not found
    Task<bool?> ToggleLikeAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);

    // Returns true = now favorited, false = now unfavorited, null = ressource or user not found
    Task<bool?> ToggleFavoriteAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);
}
