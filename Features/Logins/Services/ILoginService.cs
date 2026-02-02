using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.Logins.LoginDtos;

namespace Ressource_API.Features.Logins.Services;

public interface ILoginService
{
    Task<IEnumerable<Login>> GetAllLoginsAsync(CancellationToken cancellationToken = default);
    Task<Login?> GetLoginByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Login> CreateLoginAsync(CreateLoginDto dto, CancellationToken cancellationToken = default);
    Task<Login?> UpdateLoginAsync(int id, UpdateLoginDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteLoginAsync(int id, CancellationToken cancellationToken = default);
}
