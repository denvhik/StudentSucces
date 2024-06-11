using Amazon.S3.Model;

namespace AwsS3Service;
public interface IS3Service
{
    Task<DeleteObjectResponse> DeleteFileAsync(string fileName);
    Task<Stream> GetFileAsync(string fileName);
    Task <string>UploadFileAsync(Stream fileStream, string fileName);
}