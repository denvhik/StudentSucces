using AuthenticationWebApi.Controllers;
using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;
using BllAuth.Services.AuthService;
using BllAuth.Services.EmailService;
using BllAuth.Services.GenerateTokenService;
using BllAuth.Services.LogOutService;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AuthApiControllerTest;

public class Tests
{

    private static readonly IAuthService authService = Substitute.For<IAuthService>();
    private static readonly IGenerateTokenService generateTokenService = Substitute.For<IGenerateTokenService>();
    private static readonly IMapper mapper = Substitute.For<IMapper>();
    private static readonly ILogoutService logoutService= Substitute.For<ILogoutService>();
    private static readonly IEmailService emailService = Substitute.For<IEmailService>();
    private readonly AuthenticationController authenticationController = new AuthenticationController(authService,mapper,
        generateTokenService,emailService,logoutService);
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegisterUserRequest 
        { 
            Email = "asag@gmail.com",
            UserName = "Test",
            Password = "password"
        };
        var mappedRequest = new RegisterUser 
        {
            Email = request.Email,
            UserName = request.UserName,
            Password = request.Password
        };
         mapper.Map<RegisterUser>(request).Returns(mappedRequest);
        // Act
        var result = await authenticationController.Register(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void Register_ShouldThrowException_WhenRegistrationFails()
    {
        // Arrange
        var request = new RegisterUserRequest
        {

        };
        var mappedRequest = new RegisterUser
        {

        };
        mapper.Map<RegisterUser>(request).Returns(mappedRequest);
        authService.RegisterUser(mappedRequest).Returns(Task.FromException<bool>(new Exception("Registration failed")));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await authenticationController.Register(request));
    }

    [Test]
    public async Task Login_ShouldReturnOk_WhenAuthenticationIsSuccessful()
    {
        // Arrange
        var request = new LoginUserRequest 
        {
            Email = "Des@gmail.com",
            Password = "password"
        };
        var mappedRequest = new LoginUser 
        {
            Email = request.Email,
            Password = request.Password
        };
        mapper.Map<LoginUser>(request).Returns(mappedRequest);
        authService.Login(mappedRequest).Returns(Task.FromResult(true));
        generateTokenService.GenerateAccesToken(mappedRequest).Returns(Task.FromResult("AccessToken"));
        generateTokenService.GenerateRefreshToken(mappedRequest).Returns(Task.FromResult("RefreshToken"));

        // Act
        var result = await authenticationController.Login(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as LoginResponse;
        Assert.That(response.Token, Is.EqualTo("AccessToken"));
        Assert.That(response.RefreshToken, Is.EqualTo("RefreshToken"));
    }

    [Test]
    public void Login_ShouldReturnBadRequest_WhenInvalidDataIsProvided()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = "ens@gmailcom",
            Password = "password"
        };
        var mappedRequest = new LoginUser
        {
            Email = request.Email,
            Password = request.Password
        };
        mapper.Map<LoginUser>(request).Returns(mappedRequest);
        authService.Login(mappedRequest).Returns(Task.FromException<bool>(new FormatException("Invalid email format")));

        // Act & Assert
        var ex = Assert.ThrowsAsync<FormatException>(() => authenticationController.Login(request));
        Assert.That(ex.Message, Is.EqualTo("Invalid email format"));

    }
}