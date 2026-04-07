using System.ComponentModel.DataAnnotations;

namespace Ressource_API.Features.PasswordInfos.PasswordInfoDtos;

public class CreatePasswordInfoDto
{
    [Required]
    public Guid UserId { get; set; }

    [Range(0, int.MaxValue)]
    public int AttemptCount { get; set; } = 0;
}