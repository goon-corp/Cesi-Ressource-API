using Ressource_API.Features.RessourceProgressions.Models;
using Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;

namespace Ressource_API.Features.RessourceProgressions.Services;

public interface IRessourceProgressionService
{
    Task<IEnumerable<RessourceProgression>> GetAllRessourceProgressionsAsync(CancellationToken cancellationToken = default);
    Task<RessourceProgression?> GetRessourceProgressionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RessourceProgression> CreateRessourceProgressionAsync(CreateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    Task<RessourceProgression?> UpdateRessourceProgressionAsync(int id, UpdateRessourceProgressionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceProgressionAsync(int id, CancellationToken cancellationToken = default);
}
