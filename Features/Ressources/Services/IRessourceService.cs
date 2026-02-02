using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.Ressources.RessourceDtos;

namespace Ressource_API.Features.Ressources.Services;

public interface IRessourceService
{
    Task<IEnumerable<Ressource>> GetAllRessourcesAsync(CancellationToken cancellationToken = default);
    Task<Ressource?> GetRessourceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Ressource> CreateRessourceAsync(CreateRessourceDto dto, CancellationToken cancellationToken = default);
    Task<Ressource?> UpdateRessourceAsync(int id, UpdateRessourceDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceAsync(int id, CancellationToken cancellationToken = default);
}
