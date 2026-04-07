using Ressource_API.Features.RessourceConfidentialityTypes.Extensions;
using Ressource_API.Features.Ressources.Dtos;
using Ressource_API.Features.Ressources.Models;
using Ressource_API.Features.RessourceStatuses.Extensions;
using Ressource_API.Features.RessourceTypes.Extensions;
using Ressource_API.Features.Tags.Extensions;

namespace Ressource_API.Features.Ressources.Extensions;

public static class RessourceExtensions
{
    extension(Ressource ressource)
    {
        public ReturnRessourceDto ToReturnDto()
        {
            return new()
            {
                Id = ressource.Id,
                Title = ressource.Title,
                Description = ressource.Description,
                ThumbnailId = ressource.ThumbnailId,
                Status = ressource.RessourceStatus.ToReturnDto(),
                ConfidentialityType = ressource.RessourceConfidentialityType.ToReturnDto(),
                Type = ressource.RessourceType.ToReturnDto(),
                Tags = ressource.Tags.Select(t => t.ToDto())
            };
        }
    }
}