using Amazon.S3;
using Amazon.S3.Model;


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
    /// <summary>
    /// Uploads a file to the specified S3 bucket.
    /// </summary>
    /// <param name="fileStream">The stream of the file to upload. Must not be null.</param>
    /// <param name="fileName">The name of the file to be uploaded. Must not be null.</param>
    /// <returns>
    /// A task that represents the asynchronous upload operation. 
    /// The task result contains a <see cref="string"/> which is the URL of the uploaded file.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="fileStream"/> or <paramref name="fileName"/> is null.
    /// </exception>
    /// <exception cref="AmazonS3Exception">
    /// Thrown when an S3-specific error occurs during the upload operation.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when a general error occurs during the upload operation.
    /// </exception>
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
    /// <summary>
    /// Retrieves a file from the specified S3 bucket.
    /// </summary>
    /// <param name="fileName">The name of the file to retrieve. Must not be null.</param>
    /// <returns>
    /// A task that represents the asynchronous get operation.
    /// The task result contains a <see cref="Stream"/> which is the response stream of the retrieved file.
    /// </returns>
    /// <exception cref="AmazonS3Exception">
    /// Thrown when an S3-specific error occurs during the get operation.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when a general error occurs during the get operation.
    /// </exception>
    public async Task<Stream> GetFileAsync(string fileName)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            var response = await _client.GetObjectAsync(request);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex)
        {
            throw new AmazonS3Exception ($"Failed to get {ex.Message}");
        }
    }
    /// <summary>
    /// Deletes a file from the specified S3 bucket.
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted. Must not be null.</param>
    /// <returns>
    /// A task that represents the asynchronous delete operation.
    /// The task result contains a <see cref="DeleteObjectResponse"/> which includes the HTTP status code and other response details.
    /// </returns>
    /// <exception cref="AmazonS3Exception">
    /// Thrown when an S3-specific error occurs during the delete operation.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown when a general error occurs during the delete operation, including a detailed error message.
    /// </exception>
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