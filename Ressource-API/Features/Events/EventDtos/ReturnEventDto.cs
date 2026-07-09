using Ressource_API.Features.Ressources.Dtos;

namespace Ressource_API.Features.Events.EventDtos;

public class ReturnEventDto
{
    public Guid Id { get; set; }

    public bool IsVirtual { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public string? EventLink { get; set; }

    public string Location { get; set; } = null!;
    
    public ReturnRessourceDto Ressource { get; set; } = null!;
}