using AuthenticationWebApi.Controllers;
using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;
using BllAuth.Services.AuthService;
using BllAuth.Services.EmailService;
using BllAuth.Services.GenerateTokenService;
using BllAuth.Services.LogOutService;
using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;

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

    [Test]
    public async Task RefreshToken_ShouldReturnOk_WhenTokenIsRefreshedSuccessfully()
    {
        // Arrange
        var request = new RefreshTokenRequest
        {
            AccessToken = "AccessToken",
            RefreshToken = "RefreshToken"
        };

        // Mock the behavior of GetPrincipalFromExpiredToken method
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.Email, "test@example.com")
        }));
        generateTokenService.GetPrincipalFromExpiredToken(request.AccessToken).Returns(principal);

        // Mock the behavior of GetUserByEmail method
        var user = new User { Email = "test@example.com", RefreshToken = "RefreshToken", ExpirationTimetoken = DateTime.UtcNow.AddDays(1) };
        emailService.GetUserByEmail(Arg.Any<string>()).Returns(user);

        // Mock the behavior of GenerateAccesToken method
        generateTokenService.GenerateAccesToken(Arg.Any<LoginUser>()).Returns(Task.FromResult("NewAccessToken"));

        // Mock the behavior of GenerateRefreshToken method
        generateTokenService.GenerateRefreshToken(Arg.Any<LoginUser>()).Returns(Task.FromResult("NewRefreshToken"));

        // Act
        var result = await authenticationController.RefreshToken(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var response = okResult.Value as LoginResponse;
        Assert.That(response.Token, Is.EqualTo("NewAccessToken"));
        Assert.That(response.RefreshToken, Is.EqualTo("NewRefreshToken"));
    }
    [Test]
    public async Task ChangePassword_ShouldReturnOk_WhenPasswordIsChangedSuccessfully()
    {
        // Arrange
        var changePassword = new ChangePassword
        {
            Email = "test@example.com",
            CurrentPassword = "oldpassword",
            NewPassword = "newpassword"
        };

        // Mock the behavior of ChangePasswordAsync method
        var result = IdentityResult.Success;
        authService.ChangePasswordAsync(changePassword.Email, changePassword.CurrentPassword, changePassword.NewPassword).Returns(Task.FromResult(result));

        // Act
        var response = await authenticationController.ChangePassword(changePassword);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(response);
        var okResult = response as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo("Password changed successfully"));
    }

    [Test]
    public async Task ChangePassword_ShouldReturnBadRequest_WhenRequestDataIsNull()
    {
        // Act
        var response = await authenticationController.ChangePassword(null);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(response);
        var badRequestResult = response as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Invalid request data"));
    }

    [Test]
    public async Task ChangePassword_ShouldReturnBadRequest_WhenChangePasswordFails()
    {
        // Arrange
        var changePassword = new ChangePassword
        {
            Email = "test@example.com",
            CurrentPassword = "oldpassword",
            NewPassword = "newpassword"
        };

        // Mock the behavior of ChangePasswordAsync method to simulate failure
        var errors = new List<IdentityError> { new IdentityError { Description = "Password change failed" } };
        var result = IdentityResult.Failed(errors.ToArray());
        authService.ChangePasswordAsync(changePassword.Email, changePassword.CurrentPassword, changePassword.NewPassword).Returns(Task.FromResult(result));

        // Act
        var response = await authenticationController.ChangePassword(changePassword);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(response);
        var badRequestResult = response as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(errors));
    }
}