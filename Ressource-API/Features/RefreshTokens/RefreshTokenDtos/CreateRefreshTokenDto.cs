using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.RefreshTokens.RefreshTokenDtos;

public class CreateRefreshTokenDto
{
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public DateTime ExpirationTime { get; set; }

    [Required]
    public Guid UserId { get; set; }
}