using System.Security.Claims;
using Ressource_API.Features.Authentifications.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ressource_API.Features.Authentifications.AuthentificationDtos;
using Ressource_API.Features.Authentifications.Services;

namespace Ressource_API.Features.Authentifications;

[ApiController]
[Route("api/auth")]
public class AuthentificationController : ControllerBase
{
    private readonly IAuthentificationService _service;
    private readonly ILogger<AuthentificationController> _logger;

    public AuthentificationController(IAuthentificationService service, ILogger<AuthentificationController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _service.RegisterUser(dto);

        return result.Match<IActionResult>(
            onSuccess: () => Created(),
            onFailure: error => BadRequest(new { error })
        );
    }

    [HttpPut("confirm-account/{token}")]
    public async Task<IActionResult> ConfirmAccount(string token)
    {
        var result = await _service.ConfirmAccount(token);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new { message = "Account activated successfully." }),
            onFailure: error => BadRequest(new { error })
        );
    }

    [HttpPost("login/{client}")]
    public async Task<IActionResult> Login(
        [FromRoute] string client,
        [FromBody] LoginDto dto)
    {
        var allowedClients = new[] { "web", "mobile" };
        //shut down the request before entering login if the client type is not good
        if (!allowedClients.Contains(client)) throw new Exception("Invalid client type");
        
        var result = await _service.Login(dto);

        return result.Match<IActionResult>(
            onSuccess: tokens =>
            {
                if(client.ToLower() ==
                    "web") //request from the backoffice OR the web app, return token in httpOnly cookie
                {
                    Response.Cookies.Append("refresh_token", tokens.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(15),
                        Path = "/api/auth/refresh-token"
                    });

                    return Ok(new { accessToken = tokens.AccessToken });
                }

                return Ok(result); //return the token in the body of the result, for mobile apps
            },
            onFailure: error => Unauthorized(new { error })
        );
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var result = await _service.ForgotPassword(dto);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new
                { message = "If an account with that email exists, a password reset link has been sent." }),
            onFailure: error => BadRequest(new { error })
        );
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _service.ResetPassword(dto);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new
                { message = "Password has been reset successfully. You can now login with your new password." }),
            onFailure: error => BadRequest(new { error })
        );
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _service.RefreshToken(dto);

        return result.Match<IActionResult>(
            onSuccess: tokens => Ok(tokens),
            onFailure: error => Unauthorized(new { error })
        );
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenDto dto)
    {
        var result = await _service.Logout(dto.RefreshToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new { message = "Logged out successfully." }),
            onFailure: error => BadRequest(new { error })
        );
    }

    [Authorize]
    [HttpGet("token")]
    public async Task<IActionResult> GetActiveSessions([FromHeader(Name = "x-refresh-token")] string refreshToken)
    {
        var userId = GetUserId();
        var result = await _service.GetActiveSessions(userId, refreshToken);

        return result.Match<IActionResult>(
            onSuccess: tokens => Ok(tokens),
            onFailure: error => BadRequest(new { error })
        );
    }

    [Authorize]
    [HttpDelete("token/{refreshTokenId:guid}")]
    public async Task<IActionResult> RevokeSession(Guid tokenId)
    {
        var userId = GetUserId();
        var result = await _service.RevokeToken(userId, tokenId);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new { message = "Token revoked successfully." }),
            onFailure: error => NotFound(new { error })
        );
    }

    [Authorize]
    [HttpDelete("tokens")]
    public async Task<IActionResult> RevokeAllOtherSessions([FromHeader(Name = "x-refresh-token")] string refreshToken)
    {
        var userId = GetUserId();
        var result = await _service.RevokeAllOtherSessions(userId, refreshToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new { message = "All other tokens revoked successfully." }),
            onFailure: error => BadRequest(new { error })
        );
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordDto dto,
        [FromHeader(Name = "x-refresh-token")] string refreshToken)
    {
        var userId = GetUserId();
        var result = await _service.ChangePassword(userId, dto, refreshToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(new { message = "Password changed successfully." }),
            onFailure: error => BadRequest(new { error })
        );
    }
}