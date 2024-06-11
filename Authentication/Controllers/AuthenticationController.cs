using AuthenticationWebApi.Models;
using AutoMapper;
using BllAuth.Models;
using BllAuth.Services.AuthService;
using BllAuth.Services.EmailService;
using BllAuth.Services.GenerateTokenService;
using BllAuth.Services.LogOutService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthenticationWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogoutService _logOut;
    public AuthenticationController(IAuthService authService,
        IMapper mapper,
        IGenerateTokenService generateTokenService,
        IEmailService emailService,
        ILogoutService logOut)
    {

        _authService = authService;
        _mapper = mapper;
        _generateTokenService = generateTokenService;
        _emailService = emailService;
        _logOut = logOut;
    }

    /// <summary>
    /// Registers a new user with the given user details.
    /// </summary>
    /// <param name="request">The user registration details.</param>
    /// <returns>A success message if the registration is successful.</returns>
    /// <exception cref="Exception">Thrown if there is an issue with the user registration process.</exception>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        try
        {
            var mappedRequest = _mapper.Map<RegisterUser>(request);
            return Ok(await _authService.RegisterUser(mappedRequest));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// Authenticates a user and generates an access token and a refresh token.
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <returns>An access token and a refresh token if authentication is successful, otherwise unauthorized.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        if (request is null) 
        {
            return BadRequest("Invalid requst");
        }
        var mappedRequest = _mapper.Map<LoginUser>(request);
        if (await _authService.Login(mappedRequest))
        {
            var token = await _generateTokenService.GenerateAccesToken(mappedRequest);
            var refreshtoken = await _generateTokenService.GenerateRefreshToken(mappedRequest);
            return Ok(new LoginResponse 
            {
                Token = token,
                RefreshToken = refreshtoken

            });
        }
        return Unauthorized();
    }
    /// <summary>
    /// Registers a new administrator with the provided details.
    /// </summary>
    /// <param name="request">Admin registration details.</param>
    /// <returns>A success message if the registration is successful.</returns>
    /// <exception cref="Exception">Thrown if there is an error during the registration process.</exception>
    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserRequest request)
    {
        try
        {
            var mappedRequest = _mapper.Map<RegisterUser>(request);
            return Ok(await _authService.RegisterAdmin(mappedRequest));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// Refreshes the authentication tokens by validating the old tokens and issuing new ones.
    /// </summary>
    /// <param name="request">The refresh token and access token information.</param>
    /// <returns>Newly generated access and refresh tokens.</returns>
    /// <exception cref="SecurityTokenException">Thrown if the token validation fails.</exception>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid client request");
        }

        try
        {
            var principal = _generateTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _emailService.GetUserByEmail(email);

            if (user == null || user.RefreshToken != request.RefreshToken || user.ExpirationTimetoken <= DateTime.UtcNow)
            {
                return BadRequest("Invalid client request");
            }
            var mappedrequest = _mapper.Map<LoginUser>(user);
            var newAccessToken = await _generateTokenService.GenerateAccesToken(mappedrequest);
            var newRefreshToken = await _generateTokenService.GenerateRefreshToken(mappedrequest);
          
            return Ok(new LoginResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (SecurityTokenException)
        {
            return BadRequest("Invalid token");
        }
    }
    /// <summary>
    /// Changes the password of a user given the old password and the new password details.
    /// </summary>
    /// <param name="changePassword">The change password details including email, current, and new password.</param>
    /// <returns>A success message if the password change is successful.</returns>
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword) 
    {
        if (changePassword == null) 
           return BadRequest("Invalid request data");

        var result = await _authService.ChangePasswordAsync(changePassword.Email, 
            changePassword.CurrentPassword, changePassword.NewPassword);
        if (!result.Succeeded) 
        {
            return BadRequest(result.Errors);
        }
        return Ok("Password changed successfully");
    }
    /// <summary>
    /// Registers a new manager with the provided user details.
    /// </summary>
    /// <param name="request">Manager registration details.</param>
    /// <returns>A success message if the registration is successful.</returns>
    /// <exception cref="Exception">Thrown if there is an error during the registration process.</exception>
    [HttpPost("register-menager")]
    public async Task<IActionResult> RegisterMenager([FromBody] RegisterUserRequest request)
    {
        try
        {
            var emailexist = await _emailService.GetUserByEmail(request.Email);

            if (emailexist != null)
                return Conflict("Email already exist");

            var mappedRequest = _mapper.Map<RegisterUser>(request);
            return Ok(await _authService.RegisterAdmin(mappedRequest));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// Logs out the current user by clearing the authentication cookies.
    /// </summary>
    /// <returns>A success message indicating the user has been logged out successfully.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _logOut.Logout();
        return Ok(new { message = "Logged out successfully" });
    }
}
