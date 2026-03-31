using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;

namespace Ressource_API.Features.RessourceProgressions.Services;

public interface IRessourceProgressionService
{
    Task<IEnumerable<RessourceProgression>> GetAllRessourceProgressionsAsync(CancellationToken cancellationToken = default);
    Task<RessourceProgression?> GetRessourceProgressionByIdAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);
    Task<RessourceProgression> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    Task<RessourceProgression?> UpdateRessourceProgressionAsync(Guid ressourceId, Guid userId, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceProgressionAsync(Guid ressourceId, Guid userId, CancellationToken cancellationToken = default);
}
