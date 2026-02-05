using Ressource_API.Common.Services;
using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.Repositories;
using Ressource_API.Features.RefreshTokens.Factories;

namespace Ressource_API.Features.RefreshTokens.Services;

/// <summary>
/// Service for managing user refresh tokens and session lifecycle
/// </summary>
public class RefreshTokenService : BaseTokenService<RefreshToken, IRefreshTokenRepository, IRefreshTokenFactory>, IRefreshTokenService
{
    public RefreshTokenService(
        IRefreshTokenRepository repository,
        IRefreshTokenFactory factory,
        ILogger<RefreshTokenService> logger)
        : base(
            repository,
            factory,
            logger,
            getTokenId: s => s.Id,
            getEntityId: s => s.UserId,
            getToken: s => s.Token,
            getConsumed: s => !s.IsActive,
            getExpiresAt: s => s.ExpirationTime,
            setConsumed: (s, consumed) => s.IsActive = !consumed,
            setUpdateTime: (s, time) => s.UpdateTime = time,
            createTokenFunc: (userId, token, expiresAt) => factory.Create(userId, token, expiresAt))
    {
    }

    // Delegate methods to base with user-specific naming
    public new async Task<RefreshToken?> GetByRefreshToken(string refreshToken) =>
        await base.GetByToken(refreshToken);

    public new async Task<RefreshToken> CreateRefreshToken(Guid userId, string refreshToken, DateTime expiresAt) =>
        await base.CreateToken(userId, refreshToken, expiresAt);

    public new async Task<bool> ConsumeRefreshToken(string refreshToken) =>
        await base.ConsumeToken(refreshToken);

    public async Task<bool> RevokeAllUserRefreshTokens(Guid userId) =>
        await base.RevokeAllEntityTokens(userId);

    public new async Task<bool> RevokeRefreshToken(Guid sessionId) =>
        await base.RevokeToken(sessionId);

    public new async Task CleanupExpiredRefreshTokens() =>
        await base.CleanupExpiredTokens();

    public async Task<List<RefreshToken>> GetActiveRefreshTokensByUserId(Guid userId) =>
        await base.GetActiveTokensByEntityId(userId);

    public async Task<bool> RevokeRefreshTokenForUser(Guid sessionId, Guid userId) =>
        await base.RevokeTokenForEntity(sessionId, userId);

    public new async Task<bool> RevokeAllRefreshTokensExceptCurrent(Guid userId, Guid currentRefreshTokenId) =>
        await base.RevokeAllTokensExceptCurrent(userId, currentRefreshTokenId);
}