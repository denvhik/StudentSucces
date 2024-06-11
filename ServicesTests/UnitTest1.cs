using Amazon.S3;
using Amazon.S3.Model;
using AwsS3Service;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Text;

namespace ServicesTests;

public class Tests
{
    private IS3Service _s3Service;
    private IAmazonS3 _client;

    [SetUp]
    public void Setup()
    {
        _client = Substitute.For<IAmazonS3>();
        _s3Service = new S3Service(_client);
    }
    [TearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
    }
    [Test]
    public async Task UploadFileAsync_UploadsFileToS3Bucket()
    {
        // Arrange
        var fileName = "test.jpg";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file content"));

        // Act
        await _s3Service.UploadFileAsync(fileStream, fileName);

        // Assert
        await _client.Received(1).PutObjectAsync(Arg.Any<PutObjectRequest>());
    }
    [Test]
    public void UploadFileAsync_ThrowsAmazonS3Exception_WhenAmazonS3ErrorOccurs()
    {
        // Arrange
        var fileName = "test.jpg";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file content"));

        _client.PutObjectAsync(Arg.Any<PutObjectRequest>()).Throws(new AmazonS3Exception("Error uploading file"));

        // Act & Assert
        Assert.ThrowsAsync<AmazonS3Exception>(async () => await _s3Service.UploadFileAsync(fileStream, fileName));
    }

    [Test]
    public void UploadFileAsync_ThrowsException_WhenUnknownErrorOccurs()
    {
        // Arrange
        var fileName = "test.jpg";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file content"));

        _client.PutObjectAsync(Arg.Any<PutObjectRequest>()).Throws(new Exception("Unknown error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _s3Service.UploadFileAsync(fileStream, fileName));
    }

    [Test]
    public void UploadFileAsync_ThrowsException_WhenFileNameIsNull()
    {
        // Arrange
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file content"));
        
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _s3Service.UploadFileAsync(fileStream, null));
    }

    [Test]
    public void UploadFileAsync_ThrowsException_WhenFileStreamIsNull()
    {
        // Arrange
        var fileName = "test.jpg";

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _s3Service.UploadFileAsync(null, fileName));
    }
}