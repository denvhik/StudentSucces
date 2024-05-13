using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace AwsS3Service;
public class S3Service :IS3Service
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName = "avatarimagebucket";
    private readonly string _region = "eu-north-1";
    public S3Service(IAmazonS3 client)
    {
        _client = client;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null) 
        throw new ArgumentNullException(nameof(fileStream));

        if (fileName == null)
            throw new ArgumentNullException(nameof(fileName));

        try
        {
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = _bucketName,
                Key = fileName,
            };

            await _client.PutObjectAsync(request);

            var url = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileName}";
            return url;
        }
        catch (AmazonS3Exception ex)
        {
          
            throw new AmazonS3Exception(ex.Message);
        }
        catch (Exception ex)
        {
            
            throw new Exception(ex.Message);
        }
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

    public async Task<DeleteObjectResponse> DeleteFileAsync(string fileName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _client.DeleteObjectAsync(request);
            return response;
        }
        catch (AmazonS3Exception ex)
        {

            throw new Exception($"Amazon S3 exception occurred: {ex.Message}", ex);
        }
        catch (Exception ex)
        {

            throw new Exception($"An error occurred while deleting file '{fileName}': {ex.Message}", ex);
        }
    }
}