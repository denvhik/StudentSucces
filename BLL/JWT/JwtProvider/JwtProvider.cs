using BLL.StudentDto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationWebApi.JWT.JwtProvider;

public class JwtProvider:IJwtProvider
{

    private readonly JwtOptions _jwtOptions;
    public JwtProvider(IConfiguration configuration)
    {

        _jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
    }
    public string GenerateToken(UserDTO user)
    {
        var claims = new List<Claim>
        {
        new Claim("userId", user.Id.ToString()),
        new Claim("username", user.UserName),
        new Claim("email", user.Email),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenValue;
    }
}
