using Ressource_API.Features.BackofficeLogs.Models;
using Ressource_API.Features.BackofficeLogs.BackofficeLogDtos;

namespace Ressource_API.Features.BackofficeLogs.Services;

public interface IBackofficeLogService
{
    Task<IEnumerable<BackofficeLog>> GetAllBackofficeLogsAsync(CancellationToken cancellationToken = default);
    Task<BackofficeLog?> GetBackofficeLogByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BackofficeLog> CreateBackofficeLogAsync(CreateBackofficeLogDto dto, CancellationToken cancellationToken = default);
    Task<BackofficeLog?> UpdateBackofficeLogAsync(int id, UpdateBackofficeLogDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteBackofficeLogAsync(int id, CancellationToken cancellationToken = default);
}
