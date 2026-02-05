using Ressource_API.Common.Data.Repositories;

namespace Ressource_API.Common.Services;

/// <summary>
/// Abstract base class providing common session management functionality for User and Admin sessions.
/// Eliminates code duplication between TokenService and AdminTokenService.
/// </summary>
public abstract class BaseTokenService<TToken, TRepository, TFactory> : IBaseTokenService<TToken>
    where TToken : class
    where TRepository : IBaseRepository<TToken>
    where TFactory : class
{
    protected readonly TRepository Repository;
    protected readonly TFactory Factory;
    protected readonly ILogger Logger;
    protected readonly Func<TToken, Guid> GetTokenId;
    protected readonly Func<TToken, Guid> GetEntityId;
    protected readonly Func<TToken, string> GetToken;
    protected readonly Func<TToken, bool> GetConsumed;
    protected readonly Func<TToken, DateTime> GetExpiresAt;
    protected readonly Action<TToken, bool> SetConsumed;
    protected readonly Action<TToken, DateTime> SetUpdateTime;
    protected readonly Func<Guid, string, DateTime, TToken> CreateTokenFunc;

    protected BaseTokenService(
        TRepository repository,
        TFactory factory,
        ILogger logger,
        Func<TToken, Guid> getTokenId,
        Func<TToken, Guid> getEntityId,
        Func<TToken, string> getToken,
        Func<TToken, bool> getConsumed,
        Func<TToken, DateTime> getExpiresAt,
        Action<TToken, bool> setConsumed,
        Action<TToken, DateTime> setUpdateTime,
        Func<Guid, string, DateTime, TToken> createTokenFunc)
    {
        Repository = repository;
        Factory = factory;
        Logger = logger;
        GetTokenId = getTokenId;
        GetEntityId = getEntityId;
        GetToken = getToken;
        GetConsumed = getConsumed;
        GetExpiresAt = getExpiresAt;
        SetConsumed = setConsumed;
        SetUpdateTime = setUpdateTime;
        CreateTokenFunc = createTokenFunc;
    }

    public async Task<TToken?> GetByToken(string refreshToken)
    {
        var session = await Repository.FirstOrDefaultAsync(s =>
            GetToken(s) == refreshToken &&
            !GetConsumed(s) &&
            GetExpiresAt(s) > DateTime.UtcNow);

        return session;
    }

    public async Task<TToken> CreateToken(Guid entityId, string refreshToken, DateTime expiresAt)
    {
        var session = CreateTokenFunc(entityId, refreshToken, expiresAt);
        await Repository.AddAsync(session);

        Logger.LogInformation("Token created for entity {EntityId}, expires at {ExpiresAt}",
            entityId, expiresAt);

        return session;
    }

    public async Task<bool> ConsumeToken(string refreshToken)
    {
        var session = await Repository.FirstOrDefaultAsync(s => GetToken(s) == refreshToken);

        if (session == null)
        {
            Logger.LogWarning("Attempted to consume non-existent session with token");
            return false;
        }

        if (GetConsumed(session))
        {
            Logger.LogWarning("Attempted to consume already consumed session");
            return false;
        }

        if (GetExpiresAt(session) < DateTime.UtcNow)
        {
            Logger.LogWarning("Attempted to consume expired session");
            return false;
        }

        SetConsumed(session, true);
        SetUpdateTime(session, DateTime.UtcNow);

        await Repository.UpdateAsync(session);

        Logger.LogInformation("Token consumed successfully");

        return true;
    }

    public async Task<bool> RevokeAllEntityTokens(Guid entityId)
    {
        var sessions = await Repository.ListAsync(s =>
            GetEntityId(s) == entityId &&
            !GetConsumed(s) &&
            GetExpiresAt(s) > DateTime.UtcNow);

        if (!sessions.Any())
        {
            Logger.LogInformation("No active sessions found for entity {EntityId}", entityId);
            return true;
        }

        foreach (var session in sessions)
        {
            SetConsumed(session, true);
            SetUpdateTime(session, DateTime.UtcNow);
            await Repository.UpdateAsync(session);
        }

        Logger.LogInformation("Revoked {Count} active sessions for entity {EntityId}",
            sessions.Count(), entityId);

        return true;
    }

    public async Task<bool> RevokeToken(Guid sessionId)
    {
        var session = await Repository.FindAsync(sessionId);

        if (session == null)
        {
            Logger.LogWarning("Attempted to revoke non-existent session {TokenId}", sessionId);
            return false;
        }

        if (GetConsumed(session))
        {
            Logger.LogInformation("Token {TokenId} already consumed", sessionId);
            return true;
        }

        SetConsumed(session, true);
        SetUpdateTime(session, DateTime.UtcNow);

        await Repository.UpdateAsync(session);

        Logger.LogInformation("Token {TokenId} revoked successfully", sessionId);

        return true;
    }

    public async Task CleanupExpiredTokens()
    {
        var expiredTokens = await Repository.ListAsync(s =>
            GetExpiresAt(s) < DateTime.UtcNow &&
            !GetConsumed(s));

        if (!expiredTokens.Any())
        {
            Logger.LogDebug("No expired sessions to cleanup");
            return;
        }

        foreach (var session in expiredTokens)
        {
            SetConsumed(session, true);
            SetUpdateTime(session, DateTime.UtcNow);
            await Repository.UpdateAsync(session);
        }

        Logger.LogInformation("Cleaned up {Count} expired sessions", expiredTokens.Count());
    }

    public async Task<List<TToken>> GetActiveTokensByEntityId(Guid entityId)
    {
        var sessions = await Repository.ListAsync(s =>
            GetEntityId(s) == entityId &&
            !GetConsumed(s) &&
            GetExpiresAt(s) > DateTime.UtcNow);

        Logger.LogInformation("Retrieved {Count} active sessions for entity {EntityId}", sessions.Count(), entityId);

        return sessions.ToList();
    }

    public async Task<bool> RevokeTokenForEntity(Guid sessionId, Guid entityId)
    {
        var sessions = await Repository.ListAsync(s =>
            GetEntityId(s) == entityId);

        var session = sessions.FirstOrDefault(s => GetTokenId(s) == sessionId);

        if (session == null)
        {
            Logger.LogWarning("Attempted to revoke non-existent session {TokenId} for entity {EntityId}", sessionId, entityId);
            return false;
        }

        if (GetConsumed(session))
        {
            Logger.LogInformation("Token {TokenId} already consumed", sessionId);
            return true;
        }

        SetConsumed(session, true);
        SetUpdateTime(session, DateTime.UtcNow);

        await Repository.UpdateAsync(session);

        Logger.LogInformation("Token {TokenId} revoked for entity {EntityId}", sessionId, entityId);

        return true;
    }

    public async Task<bool> RevokeAllTokensExceptCurrent(Guid entityId, Guid currentTokenId)
    {
        var sessions = await Repository.ListAsync(s =>
            GetEntityId(s) == entityId &&
            !GetConsumed(s) &&
            GetExpiresAt(s) > DateTime.UtcNow);

        var sessionsToRevoke = sessions.Where(s => GetTokenId(s) != currentTokenId).ToList();

        if (!sessionsToRevoke.Any())
        {
            Logger.LogInformation("No other active sessions found for entity {EntityId}", entityId);
            return true;
        }

        foreach (var session in sessionsToRevoke)
        {
            SetConsumed(session, true);
            SetUpdateTime(session, DateTime.UtcNow);
            await Repository.UpdateAsync(session);
        }

        Logger.LogInformation("Revoked {Count} sessions (excluding current) for entity {EntityId}",
            sessionsToRevoke.Count(), entityId);

        return true;
    }
}