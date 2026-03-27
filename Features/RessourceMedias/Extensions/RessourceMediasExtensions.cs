using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Models;

namespace Ressource_API.Features.RessourceMedias.Extensions;

public static class RessourceMediasExtensions
{
    extension(RessourceMedia media)
    {
        public ReturnRessourceMediaDto ToReturnDto()
        {
            return new()
            {
                Id = media.Id,
                MediaUrl = media.MediaUrl,
                MimeType = media.MimeType,
            };
        }
    }
}