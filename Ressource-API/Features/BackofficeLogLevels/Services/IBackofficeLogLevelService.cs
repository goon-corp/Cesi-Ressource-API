using Ressource_API.Features.BackofficeLogLevels.Models;
using Ressource_API.Features.BackofficeLogLevels.BackofficeLogLevelDtos;

namespace Ressource_API.Features.BackofficeLogLevels.Services;

public interface IBackofficeLogLevelService
{
    Task<IEnumerable<BackofficeLogLevel>> GetAllBackofficeLogLevelsAsync(CancellationToken cancellationToken = default);
    Task<BackofficeLogLevel?> GetBackofficeLogLevelByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BackofficeLogLevel> CreateBackofficeLogLevelAsync(CreateBackofficeLogLevelDto dto, CancellationToken cancellationToken = default);
    Task<BackofficeLogLevel?> UpdateBackofficeLogLevelAsync(int id, UpdateBackofficeLogLevelDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteBackofficeLogLevelAsync(int id, CancellationToken cancellationToken = default);
}
