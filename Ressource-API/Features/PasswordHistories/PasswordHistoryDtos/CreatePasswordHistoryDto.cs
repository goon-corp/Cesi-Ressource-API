using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;

public class CreatePasswordHistoryDto
{
    [Required]
    public required string PasswordHash { get; set; }

    [Required]
    public Guid IdPasswordsInfos { get; set; }
}
