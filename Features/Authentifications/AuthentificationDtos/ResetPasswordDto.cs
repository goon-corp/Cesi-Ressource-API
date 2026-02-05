using System.ComponentModel.DataAnnotations;
using Ressource_API.Common.Validators;

namespace Ressource_API.Features.Authentifications.AuthentificationDtos;
public class ResetPasswordDto
{
    [Required]
    public required string Token { get; set; }

    [Required]
    [PasswordValidator]
    public required string NewPassword { get; set; }

    [Required]
    public required string ConfirmPassword { get; set; }
}
