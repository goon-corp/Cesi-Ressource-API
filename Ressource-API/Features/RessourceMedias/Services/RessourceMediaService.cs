using Ressource_API.Common.BlobStorage.Cloudflare;
using Ressource_API.Features.RessourceMedias.Dtos;
using Ressource_API.Features.RessourceMedias.Extensions;
using Ressource_API.Features.RessourceMedias.Models;
using Ressource_API.Features.RessourceMedias.Repositories;

namespace Ressource_API.Features.RessourceMedias.Services;

public class RessourceMediaService : IRessourceMediaService
{
    private readonly IRessourceMediaRepository _repository;
    private readonly ICloudflareClient _cloudflareClient;

    public RessourceMediaService(IRessourceMediaRepository repository, ICloudflareClient cloudflareClient)
    {
        _repository = repository;
        _cloudflareClient = cloudflareClient;
    }

    public async Task<ReturnRessourceMediaDto> CreateMedia(CreateRessourceMediaDto dto)
    {
        var fileName = $"ressources/medias/{dto.File.FileName}";
        await _cloudflareClient.UploadImage(dto.File, fileName);
        var bucketUrl = Environment.GetEnvironmentVariable("CLOUDFLARE_BUCKET_URL") ??
                        throw new NullReferenceException("CLOUDFLARE_BUCKET_URL");
        var fileUrl = $"{bucketUrl}/{fileName}";

        var media = new RessourceMedia()
        {
            Id = Guid.CreateVersion7(),
            MediaUrl = fileUrl,
            MimeType = dto.File.ContentType,
        };

        await _repository.AddAsync(media);
        return media.ToReturnDto();
    }

    public async Task DeleteMedia(Guid mediaId)
    {
        var media = await _repository.FindAsync(mediaId);
        if (media is null) throw new NullReferenceException("Media not found");
        await _repository.DeleteAsync(media);
    }

    public async Task<(Stream stream, string mimeType)> GetMediaStream(Guid mediaId)
    {
        var media = await _repository.FindAsync(mediaId)
                    ?? throw new NullReferenceException("Media not found");

        var bucketUrl = Environment.GetEnvironmentVariable("CLOUDFLARE_BUCKET_URL")
                        ?? throw new NullReferenceException("CLOUDFLARE_BUCKET_URL");

        var key = media.MediaUrl.Replace($"{bucketUrl}/", "");
        var stream = await _cloudflareClient.GetObject(key);

        return (stream, media.MimeType);
    }
}