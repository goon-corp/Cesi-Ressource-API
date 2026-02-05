using System.ComponentModel.DataAnnotations;
using Ressource_API.Common.Validators;

namespace Ressource_API.Features.Authentifications.AuthentificationDtos;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
    [PasswordValidator]
    [Required]
    public string Password { get; set; }
}