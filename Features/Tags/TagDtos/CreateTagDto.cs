namespace Ressource_API.Features.Tags.TagDtos;

public class CreateTagDto
{
    public DateTime CreationTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string Label { get; set; } = null!;
    
}
