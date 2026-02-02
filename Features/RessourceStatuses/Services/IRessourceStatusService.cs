using Ressource_API.Features.RessourceStatuses.Models;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;

namespace Ressource_API.Features.RessourceStatuses.Services;

public interface IRessourceStatusService
{
    Task<IEnumerable<RessourceStatus>> GetAllRessourceStatussAsync(CancellationToken cancellationToken = default);
    Task<RessourceStatus?> GetRessourceStatusByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RessourceStatus> CreateRessourceStatusAsync(CreateRessourceStatusDto dto, CancellationToken cancellationToken = default);
    Task<RessourceStatus?> UpdateRessourceStatusAsync(int id, UpdateRessourceStatusDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRessourceStatusAsync(int id, CancellationToken cancellationToken = default);
}
