namespace Ressource_API.Features.PasswordHistories.PasswordHistoryDtos;


public class GetPasswordHistoryDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public Guid IdPasswordsInfos { get; set; }
}
