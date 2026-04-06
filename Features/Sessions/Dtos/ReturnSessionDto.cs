namespace Ressource_API.Features.Sessions.Dtos;

public class ReturnSessionDto
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid RessourceId { get; set; }
}
