using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.Sessions.Dtos;

public class CreateSessionDto
{
    [Required]
    public Guid RessourceId { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;
}
