using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ressource_API.Features.Authentifications.DTOs;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Common.Services;
using Ressource_API.Common.Services.EmailService;
using Ressource_API.Features.Authentifications.AuthentificationDtos;
using Ressource_API.Features.Logins.Repositories;
using Ressource_API.Features.Logins.Factories;
using Ressource_API.Features.Logins.Models;
using Ressource_API.Features.PasswordInfos.Repositories;
using Ressource_API.Features.PasswordInfos.Factories;
using Ressource_API.Features.PasswordHistories.Services;
using Ressource_API.Features.PasswordInfos.Models;
using Ressource_API.Features.RefreshTokens.Repositories;
using Ressource_API.Features.RefreshTokens.Services;
using Ressource_API.Features.UserRoles.Repositories;
using Ressource_API.Features.Users.Factories;
using Ressource_API.Features.Users.Models;
using Ressource_API.Features.Users.Repositories;
using Simply.Auth.AspNetCore.Models;
using Simply.Auth.Core.Abstractions;
using Simply.Auth.Core.Enums;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Ressource_API.Features.Authentifications.Services;

public class AuthentificationService : IAuthentificationService
{
    private const int MaxFailedLoginAttempts = 5;
    private const int LockoutDurationMinutes = 15;
    private const int ResetTokenExpirationMinutes = 15;

    private readonly IUserRepository _userRepository;
    private readonly ISimplyAuthService _simplyAuthService;
    private readonly IUserFactory _userFactory;
    private readonly ILoginRepository _loginRepository;
    private readonly ILoginFactory _loginFactory;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IPasswordInfoRepository _passwordInfoRepository;
    private readonly IPasswordInfoFactory _passwordInfoFactory;
    private readonly IEmailService _emailService;
    private readonly IRefreshTokenService _tokenService;
    private readonly IPasswordHistoryManager _passwordHistoryManager;
    private readonly ILogger<AuthentificationService> _logger;

    public AuthentificationService(
        IUserRepository userRepository,
        ISimplyAuthService simplyAuthService,
        IUserFactory userFactory,
        IEmailService emailService,
        IRefreshTokenService tokenService,
        IPasswordHistoryManager passwordHistoryManager,
        IUserRoleRepository userRoleRepository,
        ILoginRepository loginRepository,
        ILoginFactory loginFactory,
        IPasswordInfoRepository passwordInfoRepository,
        IPasswordInfoFactory passwordInfoFactory,
        ILogger<AuthentificationService> logger)
    {
        _userRepository = userRepository;
        _simplyAuthService = simplyAuthService;
        _userFactory = userFactory;
        _loginRepository = loginRepository;
        _loginFactory = loginFactory;
        _userRoleRepository = userRoleRepository;
        _passwordInfoRepository = passwordInfoRepository;
        _passwordInfoFactory = passwordInfoFactory;
        _emailService = emailService;
        _tokenService = tokenService;
        _passwordHistoryManager = passwordHistoryManager;
        _logger = logger;
    }

    public async Task<Result> RegisterUser(RegisterDto dto)
    {
        _logger.LogInformation("Registration attempt for email {Email}", dto.Email);

        if (dto.Password != dto.ConfirmPassword)
        {
            _logger.LogWarning("Registration failed: password mismatch for {Email}", dto.Email);
            return Result.Failure("Password must be identical.");
        }

        if (await _loginRepository.AnyAsync(l => l.Email == dto.Email))
        {
            _logger.LogWarning("Registration failed: email already exists {Email}", dto.Email);
            return Result.Failure("Email already exists");
        }

        var passwordHash = _simplyAuthService.HashPassword(dto.Password);

        var userRole = await _userRoleRepository.FirstOrDefaultAsync(r => r.RoleLabel == "Utilisateur") ?? throw new Exception("Invalid role");
        
        User newUser = _userFactory.Create(dto.UserName, dto.FirstName, dto.LastName, userRole.Id);
        newUser.ActivationCode = Guid.NewGuid();
        newUser.IsActive = false;

        var savedUser = await _userRepository.AddAsync(newUser);

        Login newLogin = _loginFactory.Create(dto.Email, passwordHash, savedUser.Id);
        await _loginRepository.AddAsync(newLogin);

        PasswordInfo passwordInfo = _passwordInfoFactory.Create(savedUser.Id);
        await _passwordInfoRepository.AddAsync(passwordInfo);

        var activationToken = newUser.ActivationCode.Value.ToString();

        await _emailService.SendRegisteringConfirmationEmail(
            activationToken,
            newUser.FirstName,
            newUser.LastName,
            dto.Email,
            "Confirmation de création de compte",
            "Confirmez votre compte");

        _logger.LogInformation("User registered successfully: {UserId}", newUser.Id);

        return Result.Success();
    }

    public async Task<Result<SimplyAuthResponse>> Login(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for email {Email}", dto.Email);

        var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email == dto.Email);

        if (login is null)
        {
            _logger.LogWarning("Login failed: login not found for {Email}", dto.Email);
            return Result.Failure<SimplyAuthResponse>("Invalid credentials");
        }

        var user = await _userRepository.FindAsync(login.UserId);

        if (user is null)
        {
            _logger.LogError("Login failed: user not found for {UserId}", login.UserId);
            return Result.Failure<SimplyAuthResponse>("Invalid credentials");
        }

        var passwordInfo = await _passwordInfoRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (passwordInfo is null)
        {
            passwordInfo = _passwordInfoFactory.Create(user.Id);
            await _passwordInfoRepository.AddAsync(passwordInfo);
        }

        var lockoutEndTime = passwordInfo.ResetDate?.AddMinutes(LockoutDurationMinutes);
        if (passwordInfo.AttemptCount >= MaxFailedLoginAttempts &&
            lockoutEndTime.HasValue &&
            lockoutEndTime.Value > DateTime.UtcNow)
        {
            var remainingMinutes = (int)Math.Ceiling((lockoutEndTime.Value - DateTime.UtcNow).TotalMinutes);
            _logger.LogWarning("Login failed: account locked for {UserId}, {Minutes} minutes remaining", user.Id,
                remainingMinutes);
            return Result.Failure<SimplyAuthResponse>(
                $"Account is locked. Please try again in {remainingMinutes} minute(s).");
        }

        if (!user.IsActive || user.ActivationCode.HasValue)
        {
            _logger.LogWarning("Login failed: account not activated for {UserId}", user.Id);
            return Result.Failure<SimplyAuthResponse>("Le compte doit être activé.");
        }

        var result = _simplyAuthService.VerifyPassword(dto.Password, login.PasswordHash);

        if (result == SimplyVerificationResult.Failed)
        {
            passwordInfo.AttemptCount++;
            passwordInfo.UpdateTime = DateTime.UtcNow;

            if (passwordInfo.AttemptCount >= MaxFailedLoginAttempts)
            {
                passwordInfo.ResetDate = DateTime.UtcNow;
                _logger.LogWarning("Account locked for {UserId} after {Attempts} failed attempts", user.Id,
                    passwordInfo.AttemptCount);
            }

            await _passwordInfoRepository.UpdateAsync(passwordInfo);
            _logger.LogWarning("Login failed: invalid password for {UserId}, attempt {Attempt}", user.Id,
                passwordInfo.AttemptCount);
            return Result.Failure<SimplyAuthResponse>("Invalid credentials");
        }

        if (passwordInfo.AttemptCount > 0)
        {
            passwordInfo.AttemptCount = 0;
            passwordInfo.ResetDate = null;
            passwordInfo.UpdateTime = DateTime.UtcNow;
            await _passwordInfoRepository.UpdateAsync(passwordInfo);
        }

        if (result == SimplyVerificationResult.SuccessRehashNeeded)
        {
            var newHash = _simplyAuthService.HashPassword(dto.Password);
            login.PasswordHash = newHash;
            login.UpdateTime = DateTime.UtcNow;
            await _loginRepository.UpdateAsync(login);
            _logger.LogInformation("Password rehashed for user {UserId}", user.Id);
        }

        var tokens = _simplyAuthService.GenerateTokens(user.Id.ToString(), new[] {new Claim(ClaimTypes.Role, "Utilisateur") });

        await _tokenService.CreateRefreshToken(
            user.Id,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiration);

        _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

        return Result.Success(new SimplyAuthResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            AccessTokenExpiration = tokens.AccessTokenExpiration,
            RefreshTokenExpiration = tokens.RefreshTokenExpiration
        });
    }

    public async Task<Result> ConfirmAccount(string token)
    {
        _logger.LogInformation("Account confirmation attempt with token {Token}", token);

        if (!Guid.TryParse(token, out var activationCode))
        {
            _logger.LogWarning("Account confirmation failed: invalid token format");
            return Result.Failure("Invalid token.");
        }

        var user = await _userRepository.FirstOrDefaultAsync(u => u.ActivationCode == activationCode);

        if (user is null)
        {
            _logger.LogWarning("Account confirmation failed: user not found for token");
            return Result.Failure("Invalid token.");
        }

        if (!user.ActivationCode.HasValue)
        {
            _logger.LogWarning("Account confirmation attempted on already activated account {UserId}", user.Id);
            return Result.Success();
        }

        user.ActivationCode = null;
        user.IsActive = true;
        user.UpdateTime = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("Account activated successfully for user {UserId}", user.Id);

        return Result.Success();
    }

    public async Task<Result> ForgotPassword(ForgotPasswordDto dto)
    {
        _logger.LogInformation("Password reset request for email {Email}", dto.Email);

        var login = await _loginRepository.FirstOrDefaultAsync(l => l.Email == dto.Email);

        if (login is null)
        {
            _logger.LogWarning("Password reset requested for non-existent email {Email}", dto.Email);
            await Task.Delay(Random.Shared.Next(100, 300));
            return Result.Success();
        }

        var user = await _userRepository.FindAsync(login.UserId);

        if (user is null || !user.IsActive || user.ActivationCode.HasValue)
        {
            _logger.LogWarning("Password reset requested for unactivated account");
            return Result.Success();
        }

        var passwordInfo = await _passwordInfoRepository.FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (passwordInfo is null)
        {
            passwordInfo = _passwordInfoFactory.Create(user.Id);
            await _passwordInfoRepository.AddAsync(passwordInfo);
        }

        var resetToken = Guid.NewGuid().ToString();
        passwordInfo.ResetToken = resetToken;
        passwordInfo.ResetDate = DateTime.UtcNow;
        passwordInfo.UpdateTime = DateTime.UtcNow;
        await _passwordInfoRepository.UpdateAsync(passwordInfo);

        var emailResult = await _emailService.SendPasswordResetEmail(
            resetToken,
            user.FirstName,
            user.LastName,
            login.Email,
            "Réinitialisation de votre mot de passe",
            "Vous avez demandé à réinitialiser votre mot de passe.",
            TimeSpan.FromMinutes(ResetTokenExpirationMinutes));

        if (!emailResult.IsSuccess)
        {
            _logger.LogError("Failed to send password reset email to {Email}: {Error}",
                login.Email, emailResult.Error);
            return Result.Failure("Failed to send reset email. Please try again later.");
        }

        _logger.LogInformation("Password reset email sent successfully to {UserId}", user.Id);
        return Result.Success();
    }

    public async Task<Result> ResetPassword(ResetPasswordDto dto)
    {
        _logger.LogInformation("Password reset attempt with token");

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            _logger.LogWarning("Password reset failed: password mismatch");
            return Result.Failure("Passwords must match.");
        }

        var passwordInfo = await _passwordInfoRepository
            .FirstOrDefaultAsync(p => p.ResetToken == dto.Token);


        if (passwordInfo is null)
        {
            _logger.LogWarning("Password reset failed: invalid token");
            return Result.Failure("Invalid or expired reset token.");
        }

        if (!passwordInfo.ResetDate.HasValue)
        {
            _logger.LogWarning("Password reset failed: token has no reset date");
            return Result.Failure("Invalid reset token.");
        }

        var tokenAge = DateTime.UtcNow - passwordInfo.ResetDate.Value;
        if (tokenAge.TotalMinutes > ResetTokenExpirationMinutes)
        {
            _logger.LogWarning("Password reset failed: token expired for user {UserId}", passwordInfo.UserId);
            return Result.Failure("This reset link has expired. Please request a new one.");
        }

        var user = await _userRepository.FindAsync(passwordInfo.UserId);

        if (user is null)
        {
            _logger.LogError("Password reset failed: user {UserId} not found for valid token", passwordInfo.UserId);
            return Result.Failure("User not found.");
        }

        var login = await _loginRepository.FirstOrDefaultAsync(l => l.UserId == user.Id);

        if (login is null)
        {
            _logger.LogError("Password reset failed: login not found for user {UserId}", user.Id);
            return Result.Failure("Login information not found.");
        }

        await _passwordHistoryManager.EnsurePasswordsInfoExistsAsync(user.Id);

        var isPasswordReused = await _passwordHistoryManager.IsPasswordReusedAsync(user.Id, dto.NewPassword);
        if (isPasswordReused)
        {
            _logger.LogWarning("Password reset failed: user {UserId} attempted to reuse a recent password", user.Id);
            return Result.Failure("You cannot reuse any of your last 5 passwords. Please choose a different password.");
        }

        await _passwordHistoryManager.AddPasswordToHistoryAsync(user.Id, login.PasswordHash);

        var newHash = _simplyAuthService.HashPassword(dto.NewPassword);

        login.PasswordHash = newHash;
        login.UpdateTime = DateTime.UtcNow;

        await _loginRepository.UpdateAsync(login);

        passwordInfo.ResetToken = null;
        passwordInfo.ResetDate = null;
        passwordInfo.UpdateTime = DateTime.UtcNow;
        await _passwordInfoRepository.UpdateAsync(passwordInfo);

        await _emailService.SendPasswordResetConfirmationEmail(
            user.FirstName,
            user.LastName,
            login.Email,
            "Votre mot de passe a été modifié",
            "Votre mot de passe a été modifié avec succès.");

        _logger.LogInformation("Password reset successful for user {UserId}", user.Id);

        return Result.Success();
    }

    public async Task<Result<SimplyAuthResponse>> RefreshToken(RefreshTokenDto dto)
    {
        _logger.LogInformation("Refresh token attempt");

        var session = await _tokenService.GetByRefreshToken(dto.RefreshToken);

        if (session is null)
        {
            _logger.LogWarning("Refresh token attempt with invalid or expired token");
            return Result.Failure<SimplyAuthResponse>("Invalid or expired refresh token.");
        }

        var user = await _userRepository.FindAsync(session.UserId);

        if (user is null)
        {
            _logger.LogError("User {UserId} not found for valid session", session.UserId);
            return Result.Failure<SimplyAuthResponse>("User not found.");
        }

        if (!user.IsActive || user.ActivationCode.HasValue)
        {
            _logger.LogWarning("Refresh token attempt for unactivated account {UserId}", user.Id);
            return Result.Failure<SimplyAuthResponse>("Account is not activated.");
        }

        await _tokenService.ConsumeRefreshToken(dto.RefreshToken);

        var tokens = _simplyAuthService.GenerateTokens(user.Id.ToString());

        await _tokenService.CreateRefreshToken(
            user.Id,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiration);

        _logger.LogInformation("Token refreshed successfully for user {UserId}", user.Id);

        return Result.Success(new SimplyAuthResponse
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            AccessTokenExpiration = tokens.AccessTokenExpiration,
            RefreshTokenExpiration = tokens.RefreshTokenExpiration
        });
    }

    public async Task<Result> Logout(string refreshToken)
    {
        _logger.LogInformation("Logout attempt");

        var consumed = await _tokenService.ConsumeRefreshToken(refreshToken);

        if (!consumed)
        {
            _logger.LogWarning("Logout attempted with invalid or already consumed refresh token");
            return Result.Success();
        }

        _logger.LogInformation("User logged out successfully");

        return Result.Success();
    }

    public async Task<Result<List<SessionDto>>> GetActiveSessions(Guid userId, string currentRefreshToken)
    {
        _logger.LogInformation("Retrieving active sessions for user {UserId}", userId);

        var sessions = await _tokenService.GetActiveRefreshTokensByUserId(userId);
        var currentSession = await _tokenService.GetByRefreshToken(currentRefreshToken);

        var sessionDtos = sessions.Select(s => new SessionDto
        {
            Id = s.Id,
            CreatedAt = s.CreationTime,
            ExpiresAt = s.ExpirationTime,
            IsCurrentSession = currentSession != null && s.Id == currentSession.Id
        }).ToList();

        return Result.Success(sessionDtos);
    }

    public async Task<Result> RevokeToken(Guid userId, Guid sessionId)
    {
        _logger.LogInformation("Revoking session {SessionId} for user {UserId}", sessionId, userId);

        var revoked = await _tokenService.RevokeRefreshTokenForUser(sessionId, userId);

        if (!revoked)
        {
            _logger.LogWarning("Session {SessionId} not found for user {UserId}", sessionId, userId);
            return Result.Failure("Session not found.");
        }

        return Result.Success();
    }

    public async Task<Result> RevokeAllOtherSessions(Guid userId, string currentRefreshToken)
    {
        _logger.LogInformation("Revoking all other sessions for user {UserId}", userId);

        var currentSession = await _tokenService.GetByRefreshToken(currentRefreshToken);

        if (currentSession == null)
        {
            _logger.LogWarning("Current session not found for user {UserId}", userId);
            return Result.Failure("Current session not found.");
        }

        await _tokenService.RevokeAllRefreshTokensExceptCurrent(userId, currentSession.Id);

        return Result.Success();
    }

    public async Task<Result> ChangePassword(Guid userId, ChangePasswordDto dto, string currentRefreshToken)
    {
        _logger.LogInformation("Password change attempt for user {UserId}", userId);

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            _logger.LogWarning("Password change failed: passwords don't match for user {UserId}", userId);
            return Result.Failure("New passwords must match.");
        }

        var user = await _userRepository.FindAsync(userId);

        if (user == null)
        {
            _logger.LogError("Password change failed: user {UserId} not found", userId);
            return Result.Failure("User not found.");
        }

        var login = await _loginRepository.FirstOrDefaultAsync(l => l.UserId == userId);

        if (login == null)
        {
            _logger.LogError("Password change failed: login not found for user {UserId}", userId);
            return Result.Failure("Login information not found.");
        }

        var verifyResult = _simplyAuthService.VerifyPassword(dto.CurrentPassword, login.PasswordHash);

        if (verifyResult == SimplyVerificationResult.Failed)
        {
            _logger.LogWarning("Password change failed: current password incorrect for user {UserId}", userId);
            return Result.Failure("Current password is incorrect.");
        }

        await _passwordHistoryManager.EnsurePasswordsInfoExistsAsync(userId);

        var isPasswordReused = await _passwordHistoryManager.IsPasswordReusedAsync(userId, dto.NewPassword);
        if (isPasswordReused)
        {
            _logger.LogWarning("Password change failed: user {UserId} attempted to reuse a recent password", userId);
            return Result.Failure("You cannot reuse any of your last 5 passwords. Please choose a different password.");
        }

        await _passwordHistoryManager.AddPasswordToHistoryAsync(userId, login.PasswordHash);

        var newHash = _simplyAuthService.HashPassword(dto.NewPassword);

        login.PasswordHash = newHash;
        login.UpdateTime = DateTime.UtcNow;

        await _loginRepository.UpdateAsync(login);

        var currentSession = await _tokenService.GetByRefreshToken(currentRefreshToken);
        if (currentSession != null)
        {
            await _tokenService.RevokeAllRefreshTokensExceptCurrent(userId, currentSession.Id);
        }

        await _emailService.SendPasswordResetConfirmationEmail(
            user.FirstName,
            user.LastName,
            login.Email,
            "Votre mot de passe a été modifié",
            "Votre mot de passe a été modifié avec succès.");

        _logger.LogInformation("Password changed successfully for user {UserId}", userId);

        return Result.Success();
    }
}