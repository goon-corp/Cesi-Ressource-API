namespace Ressource_API.Features.RessourceProgressions.RessourceProgressionDtos;

public class CreateRessourceProgressionDto
{
    public Guid RessourceId { get; set; }
    public Guid UserId { get; set; }
    public bool IsAside { get; set; }
    public bool IsExploited { get; set; }
}