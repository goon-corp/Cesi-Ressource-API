namespace Ressource_API.Features.Comments.CommentDtos;

public class CommentInfoDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public Guid RessourceId { get; set; }
    public Guid UserId { get; set; }
    public Guid? CommentId { get; set; }
}
