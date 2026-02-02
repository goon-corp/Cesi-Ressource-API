using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.RessourceMediaDtos;

namespace Ressource_API.Features.RessourceMedias.Services;

public interface IRessourceMediaService
{
    Task<IEnumerable<RessourceMedia>> GetAllRessourceMediasAsync(CancellationToken cancellationToken = default);
    Task<RessourceMedia?> GetRessourceMediaByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RessourceMedia> CreateRessourceMediaAsync(CreateRessourceMediaDto dto, CancellationToken cancellationToken = default);
    Task<RessourceMedia?> UpdateRessourceMediaAsync(int id, UpdateRessourceMediaDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceMediaAsync(int id, CancellationToken cancellationToken = default);
}
