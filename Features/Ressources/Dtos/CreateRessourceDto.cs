namespace Ressource_API.Features.Ressources.Dtos;

public class CreateRessourceDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public IEnumerable<Guid>? Tags { get; set; }
    public Guid StatusId { get; set; }
    public Guid ConfidentialityTypeId { get; set; }
    public Guid TypeId { get; set; }
}