using Ressource_API.Features.Authentifications.DTOs;
using Ressource_API.Common.ResultPattern;
using Ressource_API.Features.Authentifications.AuthentificationDtos;
using Simply.Auth.AspNetCore.Models;

namespace Ressource_API.Features.Authentifications.Services;

public interface IAuthentificationService
{
    Task<Result<SimplyAuthResponse>> Login(LoginDto dto);
    Task<Result> ConfirmAccount(string token);
    Task<Result> RegisterUser(RegisterDto dto, string roleName = "Utilisateur");
    Task<Result> ForgotPassword(ForgotPasswordDto dto);
    Task<Result> ResetPassword(ResetPasswordDto dto);
    Task<Result<SimplyAuthResponse>> RefreshToken(RefreshTokenDto dto);
    Task<Result> Logout(string refreshToken);
    Task<Result<List<SessionDto>>> GetActiveSessions(Guid userId, string currentRefreshToken);
    Task<Result> RevokeToken(Guid userId, Guid sessionId);
    Task<Result> RevokeAllOtherSessions(Guid userId, string currentRefreshToken);
    Task<Result> ChangePassword(Guid userId, ChangePasswordDto dto, string currentRefreshToken);
}
