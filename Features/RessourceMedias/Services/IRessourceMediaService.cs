using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Models;

namespace Ressource_API.Features.RessourceMedias.Services;

public interface IRessourceMediaService
{
    Task<ReturnRessourceMediaDto> CreateMedia(CreateRessourceMediaDto dto);
    Task DeleteMedia(Guid mediaId);
    Task<(Stream stream, string mimeType)> GetMediaStream(Guid mediaId);
}