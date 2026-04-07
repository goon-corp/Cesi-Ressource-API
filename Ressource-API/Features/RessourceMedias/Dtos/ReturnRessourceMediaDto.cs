namespace Ressource_API.Features.RessourceMedias.Dtos;

public class ReturnRessourceMediaDto
{
    public Guid Id { get; set; }
    public required string MediaUrl { get; set; }
    public required string MimeType { get; set; }
}