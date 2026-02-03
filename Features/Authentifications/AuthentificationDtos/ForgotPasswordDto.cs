using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.Authentifications.AuthentificationDtos;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
