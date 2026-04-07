namespace Ressource_API.Features.Ressources.Dtos;

public class UpdateRessourceDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IEnumerable<Guid> Tags { get; set; } = [];
    public Guid StatusId { get; set; }
    public Guid ConfidentialityTypeId { get; set; }
    public Guid TypeId { get; set; }
}
