namespace Ressource_API.Features.RessourceConfidentialityTypes.Models;

public partial class RessourceConfidentialityType
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Label { get; set; } = null!;
}
