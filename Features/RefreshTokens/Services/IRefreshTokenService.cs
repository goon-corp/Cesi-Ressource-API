using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.RefreshTokenDtos;

namespace Ressource_API.Features.RefreshTokens.Services;

public interface IRefreshTokenService
{
    Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RefreshToken> CreateRefreshTokenAsync(CreateRefreshTokenDto dto, CancellationToken cancellationToken = default);
    Task<RefreshToken?> UpdateRefreshTokenAsync(int id, UpdateRefreshTokenDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteRefreshTokenAsync(int id, CancellationToken cancellationToken = default);
}
