using BllAuth.Models;
using Microsoft.AspNetCore.Identity;

namespace BllAuth.Services.AuthService;

public interface IAuthService
{
    Task<bool> RegisterUser(RegisterUser user);
    Task<bool> Login(LoginUser user);
    Task<string> AddRoleAsync(string roleName);
    Task<bool> RegisterAdmin(RegisterUser user);
    Task<bool> RegisterMenaager(RegisterUser user);
    Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);
}