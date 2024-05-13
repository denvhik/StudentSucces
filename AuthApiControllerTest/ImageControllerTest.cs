using AuthenticationWebApi.Controllers;
using AwsS3Service;
using BllAuth.Services.ImageUploadService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using System.Text;

namespace AuthApiControllerTest;
public class ImageControllerTest
{
    private IS3Service _s3Service;
    private IHttpContextAccessor _httpContextAccessor;
    private IPhotoService _photoService;
    private ImageController _imageController;


    [SetUp]
    public void Setup()
    {

        
        _s3Service = Substitute.For<IS3Service>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _photoService = Substitute.For<IPhotoService>();


        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, "123") 
        };
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _httpContextAccessor.HttpContext.Returns(httpContext);

     
        _imageController = new ImageController(_photoService, _httpContextAccessor, _s3Service);
    }
    [Test]
    public async Task UploadAvatar_ReturnsOkResult_WhenFileUploaded()
    {
        // Arrange
        var fileName = "test.jpg";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Test file content"));
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", fileName);

        var resultUrl = "https://example.com/image/test.jpg";
        _s3Service.UploadFileAsync(Arg.Any<Stream>(), Arg.Any<string>()).Returns(resultUrl);

        // Act
        var result = await _imageController.UploadAvatar(formFile);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }
}

