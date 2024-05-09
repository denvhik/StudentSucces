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

    public string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

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
