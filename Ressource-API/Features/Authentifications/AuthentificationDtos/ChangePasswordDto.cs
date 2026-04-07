using System.ComponentModel.DataAnnotations;
using Ressource_API.Common.Validators;

namespace Ressource_API.Features.Authentifications.AuthentificationDtos;

public class ChangePasswordDto
{
    [Required]
    [PasswordValidator]
    public required string CurrentPassword { get; set; }

    [Required]
    [PasswordValidator]
    public required string NewPassword { get; set; }

    [Required]
    public required string ConfirmPassword { get; set; }
}
