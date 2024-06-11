using BllAuth.Models;
using Dal.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BllAuth.Services.GenerateTokenService;

public class GenerateTokenService(UserManager<User> userManager,
    IConfiguration configuration) : IGenerateTokenService
{
    /// <summary>
    /// Generates an access token for the provided login user.
    /// </summary>
    /// <param name="loginUser">An object representing the user attempting to log in, containing their email.</param>
    /// <returns>A string representing the generated access token.</returns>
    /// <exception cref="ArgumentException">Thrown when the user corresponding to the provided email is not found.</exception>
    /// <remarks>
    /// This method generates a JSON Web Token (JWT) access token for the user based on their email and associated roles.
    /// The access token contains claims such as the user's email and user ID, as well as any roles they belong to.
    /// The token is signed using a symmetric key retrieved from the application's configuration settings.
    /// </remarks>
    /// <seealso cref="LoginUser"/>
    public async Task<string> GenerateAccesToken(LoginUser loginuser)
    {
        var user = await userManager.FindByEmailAsync(loginuser.Email);

        if (user == null)
            throw new ArgumentException("User Not Found");
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim (ClaimTypes.Email,loginuser.Email),
            new Claim (ClaimTypes.NameIdentifier,user.Id.ToString()),
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:SecretKey").Value));
        SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var securitytoken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            issuer: configuration.GetSection("JwtOptions:Issuer").Value,
            audience: configuration.GetSection("JwtOptions:Audience").Value,
            signingCredentials: signingCred
            );
        string tokenstring = new JwtSecurityTokenHandler().WriteToken(securitytoken);
        return tokenstring;
    }
    /// <summary>
    /// Generates a refresh token for the provided login user.
    /// </summary>
    /// <param name="loginUser">An object representing the user attempting to log in, containing their email.</param>
    /// <returns>A string representing the generated refresh token.</returns>
    /// <exception cref="ArgumentException">Thrown when the user corresponding to the provided email is not found.</exception>
    /// <remarks>
    /// This method generates a refresh token for the user based on their email. 
    /// The refresh token is a long-lived token used to obtain a new access token when the current access token expires.
    /// The generated refresh token is stored in the user's database record and associated with an expiration time.
    /// </remarks>
    /// <seealso cref="LoginUser"/>
    public async Task<string> GenerateRefreshToken(LoginUser loginUser)
    {

        var user = await userManager.FindByEmailAsync(loginUser.Email);
        if (user == null)
            throw new ArgumentException("User Not Found");

        var refreshToken = GenerateRefreshTokenString();
        user.RefreshToken = refreshToken;
        user.ExpirationTimetoken = DateTime.UtcNow.AddDays(7); 

        await  userManager.UpdateAsync(user);

        return refreshToken;
    }
    /// <summary>
    /// Generates a new refresh token string.
    /// </summary>
    /// <returns>A string representing the generated refresh token.</returns>
    /// <remarks>
    /// This method generates a cryptographically secure random refresh token string.
    /// The generated refresh token is a base64-encoded string derived from a 32-byte random number.
    /// </remarks>
    public string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    /// <summary>
    /// Retrieves the claims principal from an expired JWT token.
    /// </summary>
    /// <param name="token">The expired JWT token as a string.</param>
    /// <returns>The claims principal extracted from the expired JWT token.</returns>
    /// <exception cref="SecurityTokenException">Thrown when the provided token is invalid or cannot be validated.</exception>
    /// <remarks>
    /// This method retrieves the claims principal from an expired JWT token without performing audience and issuer validation.
    /// It only validates the token's signature and ignores its expiration time.
    /// If the provided token is invalid or cannot be validated, a <see cref="SecurityTokenException"/> is thrown.
    /// </remarks>
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:SecretKey").Value)),
            ValidateLifetime = false 
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

}
