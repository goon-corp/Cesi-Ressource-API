using System.ComponentModel.DataAnnotations;
using Ressource_API.Common.Validators;

namespace Ressource_API.Features.Authentifications.AuthentificationDtos;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string UserName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [PasswordValidator]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;
}