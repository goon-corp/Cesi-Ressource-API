using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.PasswordInfoDtos;

namespace Ressource_API.Features.PasswordInfos.Services;

public interface IPasswordInfoService
{
    Task<IEnumerable<PasswordInfo>> GetAllPasswordInfosAsync(CancellationToken cancellationToken = default);
    Task<PasswordInfo?> GetPasswordInfoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PasswordInfo> CreatePasswordInfoAsync(CreatePasswordInfoDto dto, CancellationToken cancellationToken = default);
    Task<PasswordInfo?> UpdatePasswordInfoAsync(int id, UpdatePasswordInfoDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeletePasswordInfoAsync(int id, CancellationToken cancellationToken = default);
}
