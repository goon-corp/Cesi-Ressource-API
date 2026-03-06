using Ressource_API.Features.EmailLogs.Models;
using Ressource_API.Features.EmailLogs.EmailLogDtos;

namespace Ressource_API.Features.EmailLogs.Services;

public interface IEmailLogService
{
    Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync(CancellationToken cancellationToken = default);
    Task<EmailLog?> GetEmailLogByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddEmailLogAsync(string receiver, string content, string operationType, CancellationToken cancellationToken = default);
}
