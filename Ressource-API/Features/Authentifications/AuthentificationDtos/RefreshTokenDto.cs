using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.Authentifications.DTOs;

public class RefreshTokenDto
{
    [Required]
    public required string RefreshToken { get; set; }
}
