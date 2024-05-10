namespace AwsS3Service;
public interface IS3Service
{
    Task DeleteFileAsync(string fileName);
    Task<Stream> GetFileAsync(string fileName);
    Task UploadFileAsync(Stream fileStream, string fileName);
}