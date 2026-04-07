using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Events.EventDtos;

public class CreateEventDto
{
    public required CreateRessourceDto RessourceInfos { get; set; }
    public required bool IsVirtual { get; set; }
    public required DateTime DateStart { get; set; }
    public required DateTime DateEnd { get; set; }
    public required string? EventLink { get; set; }
    public required string Location { get; set; }
    public Guid? RessourceId { get; set; }
}