using Amazon.S3;
using Amazon.S3.Model;

namespace AwsS3Service;
public class S3Service :IS3Service
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName = "avatarimagebucket"; 

    public S3Service(IAmazonS3 client)
    {
        _client = client;
    }

    public async Task UploadFileAsync(Stream fileStream, string fileName)
    {
        var request = new PutObjectRequest
        {
            InputStream = fileStream,
            BucketName = _bucketName,
            Key = fileName
        };
        await _client.PutObjectAsync(request);
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };
        var response = await _client.GetObjectAsync(request);
        return response.ResponseStream;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };
        await _client.DeleteObjectAsync(request);
    }
}