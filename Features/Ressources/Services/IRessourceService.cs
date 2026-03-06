using Ressource_API.Common.Pagination;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.Query;
using Ressource_API.Features.Ressources.RessourceDtos;

namespace Ressource_API.Features.Ressources.Services;

public interface IRessourceService
{
    Task<PaginatedList<Ressource>> GetAllRessourcesAsync(RessourceQuery ressourceQuery,
        CancellationToken cancellationToken = default);
    Task<Ressource?> GetRessourceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Ressource> CreateRessourceAsync(CreateRessourceDto dto, CancellationToken cancellationToken = default);
    Task<Ressource?> UpdateRessourceAsync(int id, UpdateRessourceDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceAsync(int id, CancellationToken cancellationToken = default);
}
