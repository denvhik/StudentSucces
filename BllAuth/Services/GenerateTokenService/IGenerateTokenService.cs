using BllAuth.Models;
using Dal.Auth.Model;
using System.Security.Claims;

namespace BllAuth.Services.GenerateTokenService;

public interface IGenerateTokenService
{
      public Task<string> GenerateAccesToken(LoginUser loginuser);
    public Task <string> GenerateRefreshToken(LoginUser loginUser);
     public string GenerateRefreshTokenString();
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}