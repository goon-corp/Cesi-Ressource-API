using Ressource_API.Features.RefreshTokens.Models;

namespace Ressource_API.Features.RefreshTokens.Services;

/// <summary>
/// Service interface for managing user refresh tokens
/// </summary>
public interface IRefreshTokenService
{
    /// <summary>
    /// Retrieves a valid, non-consumed refresh token by token string
    /// </summary>
    Task<RefreshToken?> GetByRefreshToken(string refreshToken);

    /// <summary>
    /// Creates a new refresh token for the specified user
    /// </summary>
    Task<RefreshToken> CreateRefreshToken(Guid userId, string refreshToken, DateTime expiresAt);

    /// <summary>
    /// Marks a refresh token as consumed (used)
    /// </summary>
    Task<bool> ConsumeRefreshToken(string refreshToken);

    /// <summary>
    /// Revokes all active refresh tokens for a specific user
    /// </summary>
    Task<bool> RevokeAllUserRefreshTokens(Guid userId);

    /// <summary>
    /// Revokes a specific refresh token by its ID
    /// </summary>
    Task<bool> RevokeRefreshToken(Guid sessionId);

    /// <summary>
    /// Cleans up expired refresh tokens by marking them as consumed
    /// </summary>
    Task CleanupExpiredRefreshTokens();

    /// <summary>
    /// Gets all active (non-consumed, non-expired) refresh tokens for a user
    /// </summary>
    Task<List<RefreshToken>> GetActiveRefreshTokensByUserId(Guid userId);

    /// <summary>
    /// Revokes a specific refresh token for a specific user (security check)
    /// </summary>
    Task<bool> RevokeRefreshTokenForUser(Guid sessionId, Guid userId);

    /// <summary>
    /// Revokes all refresh tokens except the current one for a specific user
    /// </summary>
    Task<bool> RevokeAllRefreshTokensExceptCurrent(Guid userId, Guid currentRefreshTokenId);
}