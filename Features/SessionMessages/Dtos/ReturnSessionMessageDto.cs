namespace Ressource_API.Features.SessionMessages.Dtos;

public class ReturnSessionMessageDto
{
    public Guid Id { get; set; }
    public DateTime SentTime { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
}
