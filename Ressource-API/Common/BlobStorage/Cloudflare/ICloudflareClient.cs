namespace Ressource_API.Common.BlobStorage.Cloudflare;

public interface ICloudflareClient
{
    Task UploadImage(Stream image, string imageName, string type);
    Task UploadImage(IFormFile file, string fileName);
    Task<Stream> GetObject(string key);
}