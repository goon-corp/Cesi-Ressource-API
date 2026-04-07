using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;

namespace Ressource_API.Features.PasswordHistories.Services;

public interface IPasswordHistoryService
{
    Task<IEnumerable<GetPasswordHistoryDto>> GetAllAsync();
    Task<IEnumerable<GetPasswordHistoryDto>> GetByPasswordInfoIdAsync(Guid passwordInfoId);
    Task<GetPasswordHistoryDto?> GetByIdAsync(Guid id);
    Task<GetPasswordHistoryDto?> CreateAsync(CreatePasswordHistoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}
