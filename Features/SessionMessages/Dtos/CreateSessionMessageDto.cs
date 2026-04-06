using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.SessionMessages.Dtos;

public class CreateSessionMessageDto
{
    [Required]
    public Guid SessionId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}
