using Ressource_API.Features.RefreshTokens.Models;
using Ressource_API.Features.RefreshTokens.Repositories;
using Ressource_API.Features.RefreshTokens.Factories;

namespace Ressource_API.Features.RefreshTokens.Services;

/// <summary>
/// Service for managing user refresh tokens and session lifecycle
/// </summary>
public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repository;
    private readonly IRefreshTokenFactory _factory;
    private readonly ILogger<RefreshTokenService> _logger;

    public RefreshTokenService(
        IRefreshTokenRepository repository,
        IRefreshTokenFactory factory,
        ILogger<RefreshTokenService> logger)
    {
        _repository = repository;
        _factory = factory;
        _logger = logger;
    }

    public async Task<RefreshToken?> GetByRefreshToken(string refreshToken)
    {
        return await _repository.FirstOrDefaultAsync(s =>
            s.Token == refreshToken && s.IsActive && s.ExpirationTime > DateTime.UtcNow);
    }

    public async Task<RefreshToken> CreateRefreshToken(Guid userId, string refreshToken, DateTime expiresAt)
    {
        var session = _factory.Create(userId, refreshToken, expiresAt);
        await _repository.AddAsync(session);
        _logger.LogInformation("Token created for user {UserId}, expires at {ExpiresAt}", userId, expiresAt);
        return session;
    }

    public async Task<bool> ConsumeRefreshToken(string refreshToken)
    {
        var session = await _repository.FirstOrDefaultAsync(s => s.Token == refreshToken);

        if (session == null)
        {
            _logger.LogWarning("Attempted to consume non-existent session with token");
            return false;
        }

        if (!session.IsActive)
        {
            _logger.LogWarning("Attempted to consume already consumed session");
            return false;
        }

        if (session.ExpirationTime < DateTime.UtcNow)
        {
            _logger.LogWarning("Attempted to consume expired session");
            return false;
        }

        session.IsActive = false;
        session.UpdateTime = DateTime.UtcNow;
        await _repository.UpdateAsync(session);
        _logger.LogInformation("Token consumed successfully");
        return true;
    }

    public async Task<bool> RevokeAllUserRefreshTokens(Guid userId)
    {
        var sessions = await _repository.ListAsync(s =>
            s.UserId == userId && s.IsActive && s.ExpirationTime > DateTime.UtcNow);

        if (!sessions.Any())
        {
            _logger.LogInformation("No active sessions found for user {UserId}", userId);
            return true;
        }

        foreach (var session in sessions)
        {
            session.IsActive = false;
            session.UpdateTime = DateTime.UtcNow;
            await _repository.UpdateAsync(session);
        }

        _logger.LogInformation("Revoked {Count} active sessions for user {UserId}", sessions.Count, userId);
        return true;
    }

    public async Task<bool> RevokeRefreshToken(Guid sessionId)
    {
        var session = await _repository.FindAsync(sessionId);

        if (session == null)
        {
            _logger.LogWarning("Attempted to revoke non-existent session {TokenId}", sessionId);
            return false;
        }

        if (!session.IsActive)
        {
            _logger.LogInformation("Token {TokenId} already consumed", sessionId);
            return true;
        }

        session.IsActive = false;
        session.UpdateTime = DateTime.UtcNow;
        await _repository.UpdateAsync(session);
        _logger.LogInformation("Token {TokenId} revoked successfully", sessionId);
        return true;
    }

    public async Task CleanupExpiredRefreshTokens()
    {
        var expiredTokens = await _repository.ListAsync(s =>
            s.ExpirationTime < DateTime.UtcNow && s.IsActive);

        if (!expiredTokens.Any())
        {
            _logger.LogDebug("No expired sessions to cleanup");
            return;
        }

        foreach (var session in expiredTokens)
        {
            session.IsActive = false;
            session.UpdateTime = DateTime.UtcNow;
            await _repository.UpdateAsync(session);
        }

        _logger.LogInformation("Cleaned up {Count} expired sessions", expiredTokens.Count);
    }

    public async Task<List<RefreshToken>> GetActiveRefreshTokensByUserId(Guid userId)
    {
        var sessions = await _repository.ListAsync(s =>
            s.UserId == userId && s.IsActive && s.ExpirationTime > DateTime.UtcNow);

        _logger.LogInformation("Retrieved {Count} active sessions for user {UserId}", sessions.Count, userId);
        return sessions;
    }

    public async Task<bool> RevokeRefreshTokenForUser(Guid sessionId, Guid userId)
    {
        var session = await _repository.FirstOrDefaultAsync(s =>
            s.Id == sessionId && s.UserId == userId);

        if (session == null)
        {
            _logger.LogWarning("Attempted to revoke non-existent session {TokenId} for user {UserId}", sessionId, userId);
            return false;
        }

        if (!session.IsActive)
        {
            _logger.LogInformation("Token {TokenId} already consumed", sessionId);
            return true;
        }

        session.IsActive = false;
        session.UpdateTime = DateTime.UtcNow;
        await _repository.UpdateAsync(session);
        _logger.LogInformation("Token {TokenId} revoked for user {UserId}", sessionId, userId);
        return true;
    }

    public async Task<bool> RevokeAllRefreshTokensExceptCurrent(Guid userId, Guid currentRefreshTokenId)
    {
        var sessions = await _repository.ListAsync(s =>
            s.UserId == userId && s.IsActive && s.ExpirationTime > DateTime.UtcNow);

        var sessionsToRevoke = sessions.Where(s => s.Id != currentRefreshTokenId).ToList();

        if (!sessionsToRevoke.Any())
        {
            _logger.LogInformation("No other active sessions found for user {UserId}", userId);
            return true;
        }

        foreach (var session in sessionsToRevoke)
        {
            session.IsActive = false;
            session.UpdateTime = DateTime.UtcNow;
            await _repository.UpdateAsync(session);
        }

        _logger.LogInformation("Revoked {Count} sessions (excluding current) for user {UserId}",
            sessionsToRevoke.Count, userId);
        return true;
    }
}