namespace Ressource_API.Features.Comments.CommentDtos;

public class CreateCommentDto
{
    public string Content { get; set; } = null!;
    public Guid RessourceId { get; set; }
    public Guid UserId { get; set; }
    public Guid? CommentId { get; set; } // Pour les réponses à un autre commentaire
}