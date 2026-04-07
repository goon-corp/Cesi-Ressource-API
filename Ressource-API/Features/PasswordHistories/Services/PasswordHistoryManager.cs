using Ressource_API.Features.Logins.Repositories;
using Ressource_API.Features.PasswordHistories.Models;
using Ressource_API.Features.PasswordHistories.Repositories;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.PasswordInfos.Repositories;
using Ressource_API.Features.Users.Repositories;
using Simply.Auth.Core.Abstractions;


namespace Ressource_API.Features.PasswordHistories.Services;

public class PasswordHistoryManager : IPasswordHistoryManager
{
    private const int MaxPasswordHistory = 5;

    private readonly IUserRepository _userRepository;
    private readonly IPasswordInfoRepository _passwordsInfoRepository;
    private readonly IPasswordHistoryRepository _passwordHistoryRepository;
    private readonly ILoginRepository _loginRepository;
    private readonly ISimplyAuthService _authService;
    private readonly ILogger<PasswordHistoryManager> _logger;

    public PasswordHistoryManager(
        IUserRepository userRepository,
        IPasswordInfoRepository passwordsInfoRepository,
        IPasswordHistoryRepository passwordHistoryRepository,
        ISimplyAuthService authService,
        ILoginRepository loginRepository,
        ILogger<PasswordHistoryManager> logger)
    {
        _userRepository = userRepository;
        _passwordsInfoRepository = passwordsInfoRepository;
        _passwordHistoryRepository = passwordHistoryRepository;
        _loginRepository = loginRepository;
        _authService = authService;
        _logger = logger;
    }

    public async Task<bool> IsPasswordReusedAsync(Guid userId, string newPassword)
    {
        var user = await _userRepository.FindAsync(userId);
        var relatedPassword = await _passwordsInfoRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);
        var relatedLogin = await _loginRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (user == null || relatedPassword == null || relatedLogin == null)
        {
            return false;
        }

        var histories = await _passwordHistoryRepository.ListAsync(h =>
            h.PasswordInfosId == relatedPassword.Id &&
            h.DeletionTime == null);

        var recentHistories = histories
            .OrderByDescending(h => h.UpdateTime)
            .Take(MaxPasswordHistory)
            .ToList();

        foreach (var history in recentHistories)
        {
            var verificationResult = _authService.VerifyPassword(newPassword, history.PasswordHash);

            if (verificationResult != Simply.Auth.Core.Enums.SimplyVerificationResult.Failed)
            {
                _logger.LogWarning("User {UserId} attempted to reuse a recent password", userId);
                return true;
            }
        }

        var samePasswordAsOld = _authService.VerifyPassword(newPassword, relatedLogin.PasswordHash);
        if (samePasswordAsOld != Simply.Auth.Core.Enums.SimplyVerificationResult.Failed)
        {
            _logger.LogWarning("User {UserId} new password must not be the same as the actual password", userId);
            return true;
        }
        return false;
    }

    public async Task AddPasswordToHistoryAsync(Guid userId, string passwordHash)
    {
        var user = await _userRepository.FindAsync(userId);
        var relatedPassword = await _passwordsInfoRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (user == null || relatedPassword == null)
        {
            _logger.LogError("Cannot add password to history: User {UserId} infos not found", userId);
            return;
        }

        await EnsurePasswordsInfoExistsAsync(userId);

        var newHistory = new PasswordHistory()
        {
            Id = Guid.NewGuid(),
            PasswordHash = passwordHash,
            UpdateTime = DateTime.UtcNow,
            PasswordInfosId = relatedPassword.Id,
            CreationTime = DateTime.UtcNow
        };

        await _passwordHistoryRepository.AddAsync(newHistory);

        await CleanupOldPasswordHistoriesAsync(relatedPassword.Id);

        _logger.LogInformation("Password added to history for user {UserId}", userId);
    }

    public async Task EnsurePasswordsInfoExistsAsync(Guid userId)
    {
        var user = await _userRepository.FindAsync(userId);
        var relatedPassword = await _passwordsInfoRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (user == null || relatedPassword == null)
        {
            _logger.LogError("Cannot ensure PasswordsInfo: User {UserId} not found", userId);
            return;
        }

        var existingInfo = await _passwordsInfoRepository.FindAsync(relatedPassword.Id);
        if (existingInfo != null && existingInfo.DeletionTime == null)
        {
            return;
        }
    

    var passwordsInfo = new PasswordInfo()
    {
        Id = Guid.NewGuid(),
        AttemptCount = 0,
        ResetDate = DateTime.UtcNow,
        CreationTime = DateTime.UtcNow
    };

    await _passwordsInfoRepository.AddAsync(passwordsInfo);

    relatedPassword.Id = passwordsInfo.Id;
    user.UpdateTime = DateTime.UtcNow;
    await _userRepository.UpdateAsync(user);

    _logger.LogInformation("PasswordsInfo created for user {UserId}", userId);
}

private async Task CleanupOldPasswordHistoriesAsync(Guid passwordsInfoId)
{
    var histories = await _passwordHistoryRepository.ListAsync(h =>
        h.PasswordInfosId == passwordsInfoId &&
        h.DeletionTime == null);

    var orderedHistories = histories
        .OrderByDescending(h => h.UpdateTime)
        .ToList();

    var historiesToDelete = orderedHistories
        .Skip(MaxPasswordHistory)
        .ToList();

    foreach (var history in historiesToDelete)
    {
        history.DeletionTime = DateTime.UtcNow;
        history.UpdateTime = DateTime.UtcNow;
        await _passwordHistoryRepository.SoftDeleteAsync(history);
    }

    if (historiesToDelete.Any())
    {
        _logger.LogInformation("Cleaned up {Count} old password histories for PasswordsInfo {Id}",
            historiesToDelete.Count, passwordsInfoId);
    }
}

}