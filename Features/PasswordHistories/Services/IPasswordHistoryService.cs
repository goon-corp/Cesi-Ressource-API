using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;

namespace Ressource_API.Features.PasswordHistories.Services;

public interface IPasswordHistoryService
{
    Task<IEnumerable<PasswordHistory>> GetAllPasswordHistorysAsync(CancellationToken cancellationToken = default);
    Task<PasswordHistory?> GetPasswordHistoryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PasswordHistory> CreatePasswordHistoryAsync(CreatePasswordHistoryDto dto, CancellationToken cancellationToken = default);
    Task<PasswordHistory?> UpdatePasswordHistoryAsync(int id, UpdatePasswordHistoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeletePasswordHistoryAsync(int id, CancellationToken cancellationToken = default);
}
