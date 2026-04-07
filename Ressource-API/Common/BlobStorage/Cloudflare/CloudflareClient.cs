using Amazon.S3;
using Amazon.S3.Model;

namespace Ressource_API.Common.BlobStorage.Cloudflare;

public class CloudflareClient : ICloudflareClient
{
    private readonly AmazonS3Client _s3Client;
    private readonly string _bucketName;

    public CloudflareClient()
    {
        var accountId = Environment.GetEnvironmentVariable("CLOUDFLARE_ACCOUNT_ID");
        var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY_ID");
        var accessSecret = Environment.GetEnvironmentVariable("S3_SECRET_ACCESS_KEY");
        _bucketName = Environment.GetEnvironmentVariable("CLOUDFLARE_BUCKET_NAME") ??
                      throw new NullReferenceException("CLOUDFLARE_BUCKET_NAME");

        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.eu.r2.cloudflarestorage.com",
        };

        _s3Client = new AmazonS3Client(accessKey, accessSecret, config);
    }

    public async Task UploadImage(Stream image, string imageName, string type)
    {
        var request = new PutObjectRequest()
        {
            BucketName = _bucketName,
            Key = imageName,
            InputStream = image,
            ContentType = type,
            DisablePayloadSigning = true
        };

        var response = await _s3Client.PutObjectAsync(request);

        if ((int)response.HttpStatusCode >= 300)
        {
            throw new Exception($"R2 Upload Failed: {response.HttpStatusCode}");
        }
    }

    public async Task UploadImage(IFormFile file, string fileName)
    {
        using (var stream = file.OpenReadStream())
        {
            // Ensure we are at the start
            if (stream.CanSeek) stream.Position = 0;
            await UploadImage(stream, fileName, file.ContentType);
        }
    }

    public async Task<Stream> GetObject(string key)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
        };

        var response = await _s3Client.GetObjectAsync(request);
        return response.ResponseStream;
    }
}