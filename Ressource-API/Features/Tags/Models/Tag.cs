namespace Ressource_API.Features.Tags.Models;

public partial class Tag
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Label { get; set; } = null!;

}
