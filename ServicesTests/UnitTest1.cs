using Amazon.S3;
using Amazon.S3.Model;
using AwsS3Service;
using NSubstitute;
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
}