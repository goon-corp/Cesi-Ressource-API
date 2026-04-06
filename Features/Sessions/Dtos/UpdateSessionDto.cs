using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.Sessions.Dtos;

public class UpdateSessionDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
