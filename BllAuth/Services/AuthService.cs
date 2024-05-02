using BllAuth.Models;
using Dal.Auth.Model;
using DalAuth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;


namespace BllAuth.Services;
public class AuthService(UserManager<User> userManager,
    IConfiguration configuration,
    RoleManager<Roles> roleManager) : IAuthService
{
    public async Task<bool> RegisterUser(RegisterUser user)
    {
        try
        {
            var identityuser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await userManager.CreateAsync(identityuser, user.Password);
            if (result.Succeeded)
            {
                return result.Succeeded; 
            }
            else
            {
               
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                
                throw new InvalidOperationException("User creation failed: " + errors);
            }
        }
        catch (Exception ex)
        {
           
            throw new Exception("An error occurred while creating the user: " + ex.Message);
        }
    
    }
    public async Task<bool> Login(LoginUser user) 
    {
        var identityuser = await userManager.FindByEmailAsync(user.Email);
        if (identityuser == null) 
        {
            return false;
        }
        return await userManager.CheckPasswordAsync(identityuser, user.Password);
    }

    public string GenerateToken(LoginUser user)
    {
     SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:SecretKey").Value));
        var claims = new List<Claim>
            {
            new Claim (ClaimTypes.Email,user.Email),
            new Claim (ClaimTypes.Role,"Admin")
            };
        SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var securitytoken = new JwtSecurityToken(
            claims:claims,
            expires:DateTime.Now.AddHours(1),
            issuer: configuration.GetSection("JwtOptions:Issuer").Value,
            audience: configuration.GetSection("JwtOptions:Audience").Value,
            signingCredentials:signingCred
            );
        string tokenstring = new JwtSecurityTokenHandler().WriteToken(securitytoken);
        return tokenstring;
    }

    public async Task<string> AddRoleAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return "Role name is required.";
        }

        if (await roleManager.RoleExistsAsync(roleName))
        {
            return "Role already exists.";
        }
        var role = new Roles { Name = roleName };
        IdentityResult result = await roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            return $"Role {roleName} created successfully.";
        }

        return string.Join("; ", result.Errors);
    }
}
