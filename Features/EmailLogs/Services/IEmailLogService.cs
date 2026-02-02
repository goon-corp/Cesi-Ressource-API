using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;

namespace Ressource_API.Features.EmailLogs.Services;

public interface IEmailLogService
{
    Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync(CancellationToken cancellationToken = default);
    Task<EmailLog?> GetEmailLogByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<EmailLog> CreateEmailLogAsync(CreateEmailLogDto dto, CancellationToken cancellationToken = default);
    Task<EmailLog?> UpdateEmailLogAsync(int id, UpdateEmailLogDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteEmailLogAsync(int id, CancellationToken cancellationToken = default);
}
