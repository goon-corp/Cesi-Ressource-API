using Ressource_API.Features.RessourceConfidentialityTypes.RessourceConfidentialityTypeDtos;
using Ressource_API.Features.RessourceStatuses.RessourceStatusDtos;
using Ressource_API.Features.RessourceTypes.RessourceTypeDtos;
using Ressource_API.Features.Tags.TagDtos;

namespace Ressource_API.Features.Ressources.Dtos;

public class ReturnRessourceDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public Guid? ThumbnailId { get; set; }
    public RessourceStatusInfoDto? Status { get; set; }
    public RessourceConfidentialityTypeInfoDto? ConfidentialityType { get; set; }
    public RessourceTypeInfoDto? Type { get; set; }
    public IEnumerable<ReturnTagDto> Tags { get; set; } = [];
    public int LikeCount { get; set; }
    public int FavoriteCount { get; set; }
}