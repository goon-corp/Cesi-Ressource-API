using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.Logins.LoginDtos;

public class CreateLoginDto
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required] public string PasswordHash { get; set; } = null!;

    [Required] public string PasswordSalt { get; set; } = null!;

    [Required] public Guid UserId { get; set; }
}