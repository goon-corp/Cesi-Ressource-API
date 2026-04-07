namespace Ressource_API.Common.Services;

/// <summary>
/// Base interface for session management services supporting different entity types (Users, Administrators)
/// </summary>
/// <typeparam name="TToken">The session entity type (Token or AdminToken)</typeparam>
public interface IBaseTokenService<TToken> where TToken : class
{
    /// <summary>
    /// Retrieves a valid, non-consumed session by refresh token
    /// </summary>
    Task<TToken?> GetByToken(string refreshToken);

    /// <summary>
    /// Creates a new session for the specified entity
    /// </summary>
    Task<TToken> CreateToken(Guid entityId, string refreshToken, DateTime expiresAt);

    /// <summary>
    /// Marks a session as consumed (used)
    /// </summary>
    Task<bool> ConsumeToken(string refreshToken);

    /// <summary>
    /// Revokes all active sessions for a specific entity
    /// </summary>
    Task<bool> RevokeAllEntityTokens(Guid entityId);

    /// <summary>
    /// Revokes a specific session by its ID
    /// </summary>
    Task<bool> RevokeToken(Guid sessionId);

    /// <summary>
    /// Cleans up expired sessions by marking them as consumed
    /// </summary>
    Task CleanupExpiredTokens();

    /// <summary>
    /// Gets all active (non-consumed, non-expired) sessions for an entity
    /// </summary>
    Task<List<TToken>> GetActiveTokensByEntityId(Guid entityId);

    /// <summary>
    /// Revokes a specific session for a specific entity (security check)
    /// </summary>
    Task<bool> RevokeTokenForEntity(Guid sessionId, Guid entityId);

    /// <summary>
    /// Revokes all sessions except the current one for a specific entity
    /// </summary>
    Task<bool> RevokeAllTokensExceptCurrent(Guid entityId, Guid currentTokenId);
}